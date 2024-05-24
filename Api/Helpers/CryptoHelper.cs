using System.Security.Cryptography;
using System.Text;

namespace Api.Helpers;

public class CryptoHelper
{
    public static String ComputeSha256Hash(String input)
    {
        using var sha256Hash = SHA256.Create();
        var sourceBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha256Hash.ComputeHash(sourceBytes);
        return Convert.ToHexString(hashBytes);
    }
}
