namespace SingularisTestTask.Extensions;

public static class StringExtension
{
    /// <summary>
    /// Subtracts base path from full path 
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="basePath"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string GetRelativePath(this string fullPath, string basePath)
    {
        if (!fullPath.StartsWith(basePath))
        {
            throw new ArgumentException("The fullPath does not contain the basePath.");
        }

        return fullPath.Substring(basePath.Length + 1);
    }

    /// <summary>
    /// Changes host of uri
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="newHost"></param>
    /// <returns></returns>
    public static string ChangeHost(this string uri, string newHost)
    {
        
        var result = new UriBuilder(uri){Host = newHost}.Uri.ToString();
        return result[..result.Length];
    }
}