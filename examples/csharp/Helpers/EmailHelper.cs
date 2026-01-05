namespace W12CSheets.Client.Helpers;

/// <summary>
/// Email helper utilities  
/// </summary>
public static class EmailHelper
{
    /// <summary>
    /// Validate email format
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extract domain from email
    /// </summary>
    public static string GetDomain(string email)
    {
        if (IsValidEmail(email))
        {
            return email.Split('@')[1];
        }
        return string.Empty;
    }

    /// <summary>
    /// Extract username from email
    /// </summary>
    public static string GetUsername(string email)
    {
        if (IsValidEmail(email))
        {
            return email.Split('@')[0];
        }
        return string.Empty;
    }

    /// <summary>
    /// Check if email is from specific domain
    /// </summary>
    public static bool IsFromDomain(string email, string domain)
    {
        return GetDomain(email).Equals(domain, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Mask email for privacy
    /// </summary>
    public static string MaskEmail(string email)
    {
        if (!IsValidEmail(email))
            return email;

        var parts = email.Split('@');
        var username = parts[0];
        var domain = parts[1];

        if (username.Length <= 2)
            return $"{username[0]}***@{domain}";

        return $"{username[0]}***{username[^1]}@{domain}";
    }
}
