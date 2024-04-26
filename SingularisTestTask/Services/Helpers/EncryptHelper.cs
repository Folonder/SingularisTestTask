using System.Security.Cryptography;

namespace SingularisTestTask.Services.Helpers;

public static class EncryptHelper
{
    /// <summary>
    /// Reads file and converts it calculates MD5 hash of its content
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string CalculateMd5OfFile(string filePath)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(filePath);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}