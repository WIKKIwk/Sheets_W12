using System.Security.Cryptography;
using System.Text;

namespace W12CSheets.Client.Helpers;

/// <summary>
/// Cryptography helper utilities
/// </summary>
public static class CryptoHelper
{
    /// <summary>
    /// Generate random string
    /// </summary>
    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    /// <summary>
    /// Generate secure random string using RNG
    /// </summary>
    public static string GenerateSecureRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var bytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        
        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[bytes[i] % chars.Length];
        }
        
        return new string(result);
    }

    /// <summary>
    /// Hash string using SHA256
    /// </summary>
    public static string Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Generate GUID
    /// </summary>
    public static string GenerateGuid()
    {
        return Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Generate short GUID (8 characters)
    /// </summary>
    public static string GenerateShortGuid()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", "_")
            .Replace("+", "-")
            .Substring(0, 8);
    }

    /// <summary>
    /// Encode to Base64
    /// </summary>
    public static string ToBase64(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Decode from Base64
    /// </summary>
    public static string FromBase64(string base64)
    {
        var bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// Generate random number in range
    /// </summary>
    public static int GenerateRandomNumber(int min, int max)
    {
        var random = new Random();
        return random.Next(min, max + 1);
    }

    /// <summary>
    /// Generate secure random number in range using RNG
    /// </summary>
    public static int GenerateSecureRandomNumber(int min, int max)
    {
        if (min > max)
            throw new ArgumentException("Min must be less than or equal to max");

        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        var randomNumber = BitConverter.ToInt32(bytes, 0);
        
        return Math.Abs(randomNumber % (max - min + 1)) + min;
    }
}
