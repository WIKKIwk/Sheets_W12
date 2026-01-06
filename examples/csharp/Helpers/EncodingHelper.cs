namespace W12CSheets.Client.Helpers;

/// <summary>
/// Encoding helper utilities
/// </summary>
public static class EncodingHelper
{
    /// <summary>
    /// Encode string to Base64
    /// </summary>
    public static string ToBase64(string text)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Decode Base64 to string
    /// </summary>
    public static string FromBase64(string base64)
    {
        var bytes = Convert.FromBase64String(base64);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// URL encode
    /// </summary>
    public static string UrlEncode(string text)
    {
        return Uri.EscapeDataString(text);
    }

    /// <summary>
    /// URL decode
    /// </summary>
    public static string UrlDecode(string encoded)
    {
        return Uri.UnescapeDataString(encoded);
    }

    /// <summary>
    /// HTML encode
    /// </summary>
    public static string HtmlEncode(string text)
    {
        return System.Net.WebUtility.HtmlEncode(text);
    }

    /// <summary>
    /// HTML decode
    /// </summary>
    public static string HtmlDecode(string encoded)
    {
        return System.Net.WebUtility.HtmlDecode(encoded);
    }

    /// <summary>
    /// Convert to hex string
    /// </summary>
   public static string ToHex(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace("-", "");
    }

    /// <summary>
    /// Convert from hex string
    /// </summary>
    public static byte[] FromHex(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        
        return bytes;
    }
}
