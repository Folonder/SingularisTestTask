namespace SingularisTestTask.Extensions;

public static class StringExtension
{
    public static string GetRelativePath(this string fullPath, string basePath)
    {
        if (!fullPath.StartsWith(basePath))
        {
            throw new ArgumentException("The fullPath does not contain the basePath.");
        }

        return fullPath.Substring(basePath.Length + 1);
    }

    public static string ChangeHost(this string originalString, string newHost)
    {
        
        var result = new UriBuilder(originalString){Host = newHost}.Uri.ToString();
        return result[..result.Length];
    }
}