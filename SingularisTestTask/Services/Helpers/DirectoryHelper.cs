using SingularisTestTask.Extensions;

namespace SingularisTestTask.Services.Helpers;

public static class DirectoryHelper
{
    /// <summary>
    /// Get relative path of all directories in directory
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetAllDirsRelative(string directory)
    {
        return Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).Select(dir => dir.GetRelativePath(directory)).ToArray();
    }
    
    /// <summary>
    /// Get relative path of all files in directory
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    public static IEnumerable<string> GetAllFilesRelative(string directory)
    {
        return Directory.GetFiles(directory, "*", SearchOption.AllDirectories).Select(file => file.GetRelativePath(directory)).ToArray();
    }
}