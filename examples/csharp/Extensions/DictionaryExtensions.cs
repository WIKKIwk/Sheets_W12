namespace W12CSheets.Client.Extensions;

/// <summary>
/// Extension methods for Dictionary
/// </summary>
public static class DictionaryExtensions
{
    /// <summary>
    /// Get value or default
    /// </summary>
    public static TValue GetValueOrDefault<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue defaultValue = default!) where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
    }

    /// <summary>
    /// Add or update value
    /// </summary>
    public static void AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        TKey key,
        TValue value) where TKey : notnull
    {
        dictionary[key] = value;
    }

    /// <summary>
    /// Merge two dictionaries
    /// </summary>
    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        this Dictionary<TKey, TValue> first,
        Dictionary<TKey, TValue> second,
        bool overwrite = true) where TKey : notnull
    {
        var result = new Dictionary<TKey, TValue>(first);
        
        foreach (var kvp in second)
        {
            if (overwrite || !result.ContainsKey(kvp.Key))
            {
                result[kvp.Key] = kvp.Value;
            }
        }
        
        return result;
    }

    /// <summary>
    /// Remove multiple keys
    /// </summary>
    public static void RemoveAll<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        IEnumerable<TKey> keys) where TKey : notnull
    {
        foreach (var key in keys)
        {
            dictionary.Remove(key);
        }
    }

    /// <summary>
    /// Convert to query string
    /// </summary>
    public static string ToQueryString(this Dictionary<string, string> dictionary)
    {
        var pairs = dictionary.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");
        return string.Join("&", pairs);
    }
}
