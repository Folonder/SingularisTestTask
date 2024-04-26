using System.Security.Cryptography;

namespace SingularisTestTask.Services.Helpers;

public static class EncryptHelper
{
    public static string CalculateMd5OfFile(string fileName)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(fileName);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}