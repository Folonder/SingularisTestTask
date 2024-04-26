using Quartz;
using SingularisTestTask.Services.IncrementCopyService;

namespace SingularisTestTask.Workers.SchedulerWorker.Jobs;

public class IncrementCopyJob : IJob
{
    private readonly ILogger<IncrementCopyJob> _logger;
    private readonly IIncrementCopyService _incrementCopyService;
    private bool _isRunning;
        
    public IncrementCopyJob(ILogger<IncrementCopyJob> logger, IIncrementCopyService incrementCopyService)
    {
        _logger = logger;
        _incrementCopyService = incrementCopyService;
    }
        
    public Task Execute(IJobExecutionContext context)
    {
        try
        {
            var startTime = DateTime.Now;
            if (!_isRunning)
            {
                _isRunning = true;
                _logger.LogInformation($"Increment Copy Job started at {startTime}");
                _incrementCopyService.Run(startTime);
                _logger.LogInformation($"Increment Copy Job ended at {DateTime.Now}");
                _isRunning = false;
            }
            else
            {
                context.JobDetail.JobDataMap.Put("cancel", true);
                _logger.LogWarning($"Increment Copy Job with was cancelled at {startTime}");
            }

        }
        catch (ArgumentNullException e)
        {
            _logger.LogError(e.ToString());
        }
        catch (ArgumentException e)
        {
            _logger.LogError(e.Message);
        }
        catch (PathTooLongException)
        {
            _logger.LogError("The specified path, file name, or both exceed the system-defined maximum length." +
                             " The fully qualified file name must be less than 260 characters," +
                             " and the directory name must be less than 248 characters.");
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogError("Can't work with some file. Possible variants: \nProgram does not have the required permission.\n" +
                             "The file is an executable file that is in use.\nPath is a directory.\nPath specified a read-only file.");
        }
        catch (IOException e)
        {
            _logger.LogError($"Something wrong with given path. Exception message: {e.Message}");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        return Task.CompletedTask;
    }
}