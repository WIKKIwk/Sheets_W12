namespace W12CSheets.Client.Helpers;

/// <summary>
/// Comparison helper utilities
/// </summary>
public static class ComparisonHelper
{
    /// <summary>
    /// Deep compare two objects
    /// </summary>
    public static bool DeepEquals<T>(T obj1, T obj2)
    {
        if (obj1 == null && obj2 == null) return true;
        if (obj1 == null || obj2 == null) return false;
        
        var json1 = Newtonsoft.Json.JsonConvert.SerializeObject(obj1);
        var json2 = Newtonsoft.Json.JsonConvert.SerializeObject(obj2);
        
        return json1 == json2;
    }

    /// <summary>
    /// Compare collections
    /// </summary>
    public static bool CollectionsEqual<T>(IEnumerable<T> col1, IEnumerable<T> col2)
    {
        return col1.SequenceEqual(col2);
    }

    /// <summary>
    /// Compare arrays
    /// </summary>
    public static bool ArraysEqual<T>(T[] arr1, T[] arr2)
    {
        if (arr1.Length != arr2.Length) return false;
        
        for (int i = 0; i < arr1.Length; i++)
        {
            if (!Equals(arr1[i], arr2[i]))
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Compare dictionaries
    /// </summary>
    public static bool DictionariesEqual<TKey, TValue>(
        Dictionary<TKey, TValue> dict1,
        Dictionary<TKey, TValue> dict2) where TKey : notnull
    {
        if (dict1.Count != dict2.Count) return false;
        
        foreach (var kvp in dict1)
        {
            if (!dict2.TryGetValue(kvp.Key, out var value))
            {
                return false;
            }
            
            if (!Equals(kvp.Value, value))
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// Find differences between objects
    /// </summary>
    public static List<string> FindDifferences<T>(T obj1, T obj2)
    {
        var differences = new List<string>();
        var properties = typeof(T).GetProperties();
        
        foreach (var property in properties)
        {
            var value1 = property.GetValue(obj1);
            var value2 = property.GetValue(obj2);
            
            if (!Equals(value1, value2))
            {
                differences.Add($"{property.Name}: {value1} != {value2}");
            }
        }
        
        return differences;
    }
}
