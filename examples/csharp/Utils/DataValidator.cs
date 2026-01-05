namespace W12CSheets.Client.Utils;

/// <summary>
/// Data validation utilities
/// </summary>
public static class DataValidator
{
    /// <summary>
    /// Validate email format
    /// </summary>
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

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
    /// Validate cell value length
    /// </summary>
    public static bool IsValidCellValue(string value, int maxLength = 32767)
    {
        return value == null || value.Length <= maxLength;
    }

    /// <summary>
    /// Validate file name
    /// </summary>
    public static bool IsValidFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return false;

        if (fileName.Length > 255)
            return false;

        // Check for invalid characters
        var invalidChars = System.IO.Path.GetInvalidFileNameChars();
        return !fileName.Any(c => invalidChars.Contains(c));
    }

    /// <summary>
    /// Validate numeric value
    /// </summary>
    public static bool IsNumeric(string value)
    {
        return double.TryParse(value, out _);
    }

    /// <summary>
    /// Validate integer value
    /// </summary>
    public static bool IsInteger(string value)
    {
        return int.TryParse(value, out _);
    }

    /// <summary>
    /// Validate date format
    /// </summary>
    public static bool IsValidDate(string value)
    {
        return DateTime.TryParse(value, out _);
    }

    /// <summary>
    /// Validate URL format
    /// </summary>
    public static bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    /// <summary>
    /// Validate range format
    /// </summary>
    public static bool IsValidRange(string range)
    {
        if (string.IsNullOrWhiteSpace(range))
            return false;

        if (!range.Contains(':'))
        {
            return CellHelper.IsValidCellId(range);
        }

        var parts = range.Split(':');
        if (parts.Length != 2)
            return false;

        return CellHelper.IsValidCellId(parts[0]) && CellHelper.IsValidCellId(parts[1]);
    }

    /// <summary>
    /// Validate password strength
    /// </summary>
    public static (bool IsValid, string Message) ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return (false, "Password cannot be empty");

        if (password.Length < 8)
            return (false, "Password must be at least 8 characters");

        bool hasUpper = password.Any(char.IsUpper);
        bool hasLower = password.Any(char.IsLower);
        bool hasDigit = password.Any(char.IsDigit);
        bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));

        if (!hasUpper)
            return (false, "Password must contain uppercase letter");

        if (!hasLower)
            return (false, "Password must contain lowercase letter");

        if (!hasDigit)
            return (false, "Password must contain digit");

        if (!hasSpecial)
            return (false, "Password must contain special character");

        return (true, "Password is strong");
    }

    /// <summary>
    /// Sanitize cell value (remove dangerous characters)
    /// </summary>
    public static string SanitizeCellValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Remove control characters
        return new string(value.Where(c => !char.IsControl(c) || c == '\n' || c == '\r' || c == '\t').ToArray());
    }
}
