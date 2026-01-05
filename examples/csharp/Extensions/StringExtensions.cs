namespace W12CSheets.Client.Extensions;

/// <summary>
/// Extension methods for string operations
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Truncate string to specified length
    /// </summary>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        return value.Substring(0, maxLength - suffix.Length) + suffix;
    }

    /// <summary>
    /// Check if string is empty or whitespace
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>
    /// Convert to title case
    /// </summary>
    public static string ToTitleCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var textInfo = System.Globalization.CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(value.ToLower());
    }

    /// <summary>
    /// Remove all whitespace
    /// </summary>
    public static string RemoveWhitespace(this string value)
    {
        return new string(value.Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    /// <summary>
    /// Reverse string
    /// </summary>
    public static string Reverse(this string value)
    {
        char[] charArray = value.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    /// <summary>
    /// Count occurrences of substring
    /// </summary>
    public static int CountOccurrences(this string value, string substring)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(substring))
            return 0;

        int count = 0;
        int index = 0;

        while ((index = value.IndexOf(substring, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += substring.Length;
        }

        return count;
    }

    /// <summary>
    /// Check if string contains only digits
    /// </summary>
    public static bool IsNumeric(this string value)
    {
        return !string.IsNullOrEmpty(value) && value.All(char.IsDigit);
    }

    /// <summary>
    /// Check if string contains only letters
    /// </summary>
    public static bool IsAlpha(this string value)
    {
        return !string.IsNullOrEmpty(value) && value.All(char.IsLetter);
    }

    /// <summary>
    /// Check if string contains only letters and digits
    /// </summary>
    public static bool IsAlphanumeric(this string value)
    {
        return !string.IsNullOrEmpty(value) && value.All(char.IsLetterOrDigit);
    }

    /// <summary>
    /// Replace multiple spaces with single space
    /// </summary>
    public static string NormalizeWhitespace(this string value)
    {
        return System.Text.RegularExpressions.Regex.Replace(value.Trim(), @"\s+", " ");
    }
}
