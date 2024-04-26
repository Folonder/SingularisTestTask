using SingularisTestTask.Extensions;

namespace SingularisTestTask.Services.Helpers;

public static class DirectoryHelper
{
    public static IEnumerable<string> GetAllDirsRelative(string sourceFolder)
    {
        return Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories).Select(dir => dir.GetRelativePath(sourceFolder)).ToArray();
    }
    
    public static IEnumerable<string> GetAllFilesRelative(string sourceFolder)
    {
        return Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories).Select(file => file.GetRelativePath(sourceFolder)).ToArray();
    }
}