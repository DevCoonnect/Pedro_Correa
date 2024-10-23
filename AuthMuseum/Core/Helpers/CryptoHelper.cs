using System.Security.Cryptography;
using System.Text;

namespace AuthMuseum.Core.Helpers;

public static class CryptoHelper
{
    public static string ToSHA256(this string plainText)
    {
        var textBytes = Encoding.UTF8.GetBytes(plainText);
        var hashedBytes = SHA256.HashData(textBytes);
        return BitConverter.ToString(hashedBytes);
    }
}