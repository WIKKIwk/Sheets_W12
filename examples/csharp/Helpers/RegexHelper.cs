namespace W12CSheets.Client.Helpers;

/// <summary>
/// Regular expression helper utilities
/// </summary>
public static class RegexHelper
{
    /// <summary>
    /// Check if string matches pattern
    /// </summary>
    public static bool IsMatch(string input, string pattern)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(input, pattern);
    }

    /// <summary>
    /// Extract all matches
    /// </summary>
    public static List<string> GetMatches(string input, string pattern)
    {
        var matches = System.Text.RegularExpressions.Regex.Matches(input, pattern);
        return matches.Select(m => m.Value).ToList();
    }

    /// <summary>
    /// Replace all matches
    /// </summary>
    public static string ReplaceAll(string input, string pattern, string replacement)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, pattern, replacement);
    }

    /// <summary>
    /// Extract email addresses from text
    /// </summary>
    public static List<string> ExtractEmails(string text)
    {
        const string pattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b";
        return GetMatches(text, pattern);
    }

    /// <summary>
    /// Extract URLs from text
    /// </summary>
    public static List<string> ExtractUrls(string text)
    {
        const string pattern = @"https?://[^\s]+";
        return GetMatches(text, pattern);
    }

    /// <summary>
    /// Extract phone numbers from text
    /// </summary>
    public static List<string> ExtractPhoneNumbers(string text)
    {
        const string pattern = @"\b\d{3}[-.]?\d{3}[-.]?\d{4}\b";
        return GetMatches(text, pattern);
    }

    /// <summary>
    /// Remove HTML tags
    /// </summary>
    public static string RemoveHtmlTags(string html)
    {
        return ReplaceAll(html, @"<[^>]*>", "");
    }

    /// <summary>
    /// Validate credit card number (basic Luhn check)
    /// </summary>
    public static bool IsValidCreditCard(string number)
    {
        number = number.Replace(" ", "").Replace("-", "");
        
        if (!IsMatch(number, @"^\d{13,19}$"))
            return false;
        
        int sum = 0;
        bool alternate = false;
        
        for (int i = number.Length - 1; i >= 0; i--)
        {
            int digit = number[i] - '0';
            
            if (alternate)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }
            
            sum += digit;
            alternate = !alternate;
        }
        
        return sum % 10 == 0;
    }
}
