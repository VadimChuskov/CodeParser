using System.Security.Cryptography;
using System.Text;

namespace CodeParser.Common;

public class ShaManipulator
{
    public static string GenerateSha256String(string inputString)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(inputString);
        var hash = sha256.ComputeHash(bytes);
        return GetStringFromHash(hash);
    }
    
    public static string GetCheckSum(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var fileStream = File.OpenRead(filePath);
        return GetStringFromHash(sha256.ComputeHash(fileStream));
    }

    private static string GetStringFromHash(byte[] hash)
    {
        var result = new StringBuilder();
        foreach (var t in hash)
        {
            result.Append(t.ToString("X2"));
        }

        return result.ToString();
    }
}