namespace W12CSheets.Client.Helpers;

/// <summary>
/// Guard clauses for defensive programming
/// </summary>
public static class Guard
{
    /// <summary>
    /// Guard against null
    /// </summary>
    public static void AgainstNull<T>(T value, string parameterName) where T : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    /// <summary>
    /// Guard against null or empty string
    /// </summary>
    public static void AgainstNullOrEmpty(string value, string parameterName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Value cannot be null or empty", parameterName);
        }
    }

    /// <summary>
    /// Guard against null or whitespace string
    /// </summary>
    public static void AgainstNullOrWhiteSpace(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace", parameterName);
        }
    }

    /// <summary>
    /// Guard against negative numbers
    /// </summary>
    public static void AgainstNegative(int value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Value cannot be negative");
        }
    }

    /// <summary>
    /// Guard against zero
    /// </summary>
    public static void AgainstZero(int value, string parameterName)
    {
        if (value == 0)
        {
            throw new ArgumentException("Value cannot be zero", parameterName);
        }
    }

    /// <summary>
    /// Guard against out of range
    /// </summary>
    public static void AgainstOutOfRange<T>(T value, T min, T max, string parameterName) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0 || value.CompareTo(max) > 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, $"Value must be between {min} and {max}");
        }
    }

    /// <summary>
    /// Guard against empty collection
    /// </summary>
    public static void AgainstEmptyCollection<T>(IEnumerable<T> collection, string parameterName)
    {
        if (collection == null || !collection.Any())
        {
            throw new ArgumentException("Collection cannot be null or empty", parameterName);
        }
    }
}
