using Microsoft.Extensions.Options;
using SingularisTestTask.Infrastructure;
using SingularisTestTask.Models;
using SingularisTestTask.Services.Helpers;

namespace SingularisTestTask.Services.IncrementCopyService;

public class IncrementCopyService : IIncrementCopyService
{
    private readonly ILogger<IncrementCopyService> _logger;
    private readonly IIncrementCopyRepository _repository;
    private readonly string _sourceFolder;
    private readonly IncrementCopyModel _model;
    private readonly string _baseFolder;
    private DateTime _startTime;

    public IncrementCopyService(ILogger<IncrementCopyService> logger, IOptions<IncrementCopyServiceOptions> options,
        IIncrementCopyRepository repository)
    {
        _logger = logger;
        _repository = repository;
        _sourceFolder = options.Value.SourceFolder;
        _model = repository.Get(options.Value.DestinationFolder).Result ??
                 new IncrementCopyModel(options.Value.DestinationFolder);
        _baseFolder = Path.Combine(options.Value.DestinationFolder, "base");
        ValidatePaths();
    }

    public void Run(DateTime startTime)
    {
        _startTime = startTime;
        if (_repository.Get(_model.DestinationFolder).Result == null)
        {
            if (!Directory.Exists(_baseFolder))
            {
                CopyDirsAndFiles(DirectoryHelper.GetAllDirsRelative(_sourceFolder),
                    DirectoryHelper.GetAllFilesRelative(_sourceFolder), _sourceFolder, _baseFolder);
                _logger.LogInformation($"Create base folder at {startTime}");
                SetDirsAndFiles(_baseFolder);
                return;
            }

            SetDirsAndFiles(_baseFolder);
        }

        MakeIncrement(startTime);
    }

    private void MakeIncrement(DateTime startTime)
    {
        var (dirDifference, fileDifference) = GetDifference();
        if (dirDifference.Any() || fileDifference.Any())
        {
            CopyDirsAndFiles(dirDifference, fileDifference, _sourceFolder,Path.Combine(_model.DestinationFolder, _startTime.ToString("inc_yyyy_MM_dd_HH_mm_ss")));
            _logger.LogInformation($"Create increment folder at {startTime}");
            SetDirsAndFiles(_sourceFolder);
        }
    }

    private void SetDirsAndFiles(string folder)
    {
        _model.Dirs = new HashSet<string>(DirectoryHelper.GetAllDirsRelative(folder));

        var files = DirectoryHelper.GetAllFilesRelative(folder);
        foreach (var file in files)
        {
            _model.Files[file] = EncryptHelper.CalculateMd5OfFile(Path.Combine(folder, file));
        }

        _repository.CreateOrUpdateAsync(_model);
    }

    private (IEnumerable<string>, IEnumerable<string>) GetDifference()
    {
        var dirDifference = new HashSet<string>(DirectoryHelper.GetAllDirsRelative(_sourceFolder)).Except(_model.Dirs);
        var fileDifference = new HashSet<string>(DirectoryHelper.GetAllFilesRelative(_sourceFolder));
        foreach (var file in _model.Files)
        {
            if (fileDifference.Contains(file.Key) &&
                file.Value == EncryptHelper.CalculateMd5OfFile(Path.Combine(_sourceFolder, file.Key)))
            {
                fileDifference.Remove(file.Key);
            }
        }

        return (dirDifference, fileDifference);
    }

    private static void CopyDirsAndFiles(IEnumerable<string> dirs, IEnumerable<string> files, string source,
        string destination)
    {
        foreach (var dir in dirs)
        {
            Directory.CreateDirectory(Path.Combine(destination, dir));
        }

        foreach (var file in files)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(destination, file)) ??
                                      throw new InvalidOperationException());
            File.Copy(Path.Combine(source, file), Path.Combine(destination, file), true);
        }
    }

    private void ValidatePaths()
    {
        if (!Directory.Exists(_sourceFolder))
        {
            _logger.LogError($"There is no such folder to be source: {_sourceFolder}");
            Environment.Exit(1);
        }
        if (_sourceFolder == _model.DestinationFolder)
        {
            _logger.LogError($"Source folder can't be destination folder: {_sourceFolder}");
            Environment.Exit(1);
        }
        if (!Directory.Exists(_model.DestinationFolder))
        {
            _logger.LogWarning($"There is no such folder to be destination: {_model.DestinationFolder}");
            Directory.CreateDirectory(_model.DestinationFolder);
            _logger.LogWarning($"Destination directory was created with path: {_model.DestinationFolder}");
        }
    }
}