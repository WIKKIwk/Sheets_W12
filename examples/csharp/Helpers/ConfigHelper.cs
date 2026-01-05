namespace W12CSheets.Client.Helpers;

/// <summary>
/// Configuration helper for reading app settings
/// </summary>
public static class ConfigHelper
{
    private static readonly Dictionary<string, string> _config = new();

    /// <summary>
    /// Set configuration value
    /// </summary>
    public static void Set(string key, string value)
    {
        _config[key] = value;
    }

    /// <summary>
    /// Get configuration value
    /// </summary>
    public static string? Get(string key, string? defaultValue = null)
    {
        return _config.TryGetValue(key, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Get configuration value as int
    /// </summary>
    public static int GetInt(string key, int defaultValue = 0)
    {
        var value = Get(key);
        return int.TryParse(value, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// Get configuration value as bool
    /// </summary>
    public static bool GetBool(string key, bool defaultValue = false)
    {
        var value = Get(key);
        return bool.TryParse(value, out var result) ? result : defaultValue;
    }

    /// <summary>
    /// Check if key exists
    /// </summary>
    public static bool HasKey(string key)
    {
        return _config.ContainsKey(key);
    }

    /// <summary>
    /// Remove configuration key
    /// </summary>
    public static void Remove(string key)
    {
        _config.Remove(key);
    }

    /// <summary>
    /// Clear all configuration
    /// </summary>
    public static void Clear()
    {
        _config.Clear();
    }

    /// <summary>
    /// Load configuration from dictionary
    /// </summary>
    public static void LoadFrom(Dictionary<string, string> config)
    {
        foreach (var kvp in config)
        {
            _config[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// Get all configuration as dictionary
    /// </summary>
    public static Dictionary<string, string> GetAll()
    {
        return new Dictionary<string, string>(_config);
    }
}
