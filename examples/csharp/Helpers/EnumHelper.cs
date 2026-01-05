namespace W12CSheets.Client.Helpers;

/// <summary>
/// Enum helper utilities
/// </summary>
public static class EnumHelper
{
    /// <summary>
    /// Get all values of enum
    /// </summary>
    public static T[] GetValues<T>() where T : struct, Enum
    {
        return Enum.GetValues<T>();
    }

    /// <summary>
    /// Get all names of enum
    /// </summary>
    public static string[] GetNames<T>() where T : struct, Enum
    {
        return Enum.GetNames<T>();
    }

    /// <summary>
    /// Parse string to enum
    /// </summary>
    public static T? Parse<T>(string value, bool ignoreCase = true) where T : struct, Enum
    {
        if (Enum.TryParse<T>(value, ignoreCase, out var result))
        {
            return result;
        }
        return null;
    }

    /// <summary>
    /// Get enum description attribute
    /// </summary>
    public static string GetDescription<T>(T value) where T : struct, Enum
    {
        var field = value.GetType().GetField(value.ToString());
        var attributes = field?.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
        
        if (attributes?.Length > 0)
        {
            return ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
        }
        
        return value.ToString();
    }

    /// <summary>
    /// Check if enum has flag
    /// </summary>
    public static bool HasFlag<T>(T value, T flag) where T : struct, Enum
    {
        return value.HasFlag(flag);
    }

    /// <summary>
    /// Convert enum to dictionary
    /// </summary>
    public static Dictionary<int, string> ToDictionary<T>() where T : struct, Enum
    {
        return Enum.GetValues<T>()
            .ToDictionary(e => Convert.ToInt32(e), e => e.ToString());
    }
}
