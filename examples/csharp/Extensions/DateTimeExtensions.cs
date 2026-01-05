namespace W12CSheets.Client.Extensions;

/// <summary>
/// Extension methods for DateTime operations
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Check if date is today
    /// </summary>
    public static bool IsToday(this DateTime date)
    {
        return date.Date == DateTime.Today;
    }

    /// <summary>
    /// Check if date is in the past
    /// </summary>
    public static bool IsPast(this DateTime date)
    {
        return date < DateTime.Now;
    }

    /// <summary>
    /// Check if date is in the future
    /// </summary>
    public static bool IsFuture(this DateTime date)
    {
        return date > DateTime.Now;
    }

    /// <summary>
    /// Get start of day
    /// </summary>
    public static DateTime StartOfDay(this DateTime date)
    {
        return date.Date;
    }

    /// <summary>
    /// Get end of day
    /// </summary>
    public static DateTime EndOfDay(this DateTime date)
    {
        return date.Date.AddDays(1).AddTicks(-1);
    }

    /// <summary>
    /// Get start of week (Monday)
    /// </summary>
    public static DateTime StartOfWeek(this DateTime date)
    {
        int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    /// <summary>
    /// Get end of week (Sunday)
    /// </summary>
    public static DateTime EndOfWeek(this DateTime date)
    {
        return date.StartOfWeek().AddDays(7).AddTicks(-1);
    }

    /// <summary>
    /// Get start of month
    /// </summary>
    public static DateTime StartOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    /// <summary>
    /// Get end of month
    /// </summary>
    public static DateTime EndOfMonth(this DateTime date)
    {
        return date.StartOfMonth().AddMonths(1).AddTicks(-1);
    }

    /// <summary>
    /// Get age from date of birth
    /// </summary>
    public static int GetAge(this DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    /// <summary>
    /// Convert to Unix timestamp
    /// </summary>
    public static long ToUnixTimestamp(this DateTime date)
    {
        return ((DateTimeOffset)date).ToUnixTimeSeconds();
    }

    /// <summary>
    /// Format as relative time (e.g., "2 hours ago")
    /// </summary>
    public static string ToRelativeTime(this DateTime date)
    {
        var timeSpan = DateTime.Now - date;

        if (timeSpan.TotalSeconds < 60)
            return $"{(int)timeSpan.TotalSeconds} seconds ago";
        
        if (timeSpan.TotalMinutes < 60)
            return $"{(int)timeSpan.TotalMinutes} minutes ago";
        
        if (timeSpan.TotalHours < 24)
            return $"{(int)timeSpan.TotalHours} hours ago";
        
        if (timeSpan.TotalDays < 7)
            return $"{(int)timeSpan.TotalDays} days ago";
        
        if (timeSpan.TotalDays < 30)
            return $"{(int)(timeSpan.TotalDays / 7)} weeks ago";
        
        if (timeSpan.TotalDays < 365)
            return $"{(int)(timeSpan.TotalDays / 30)} months ago";
        
        return $"{(int)(timeSpan.TotalDays / 365)} years ago";
    }
}
