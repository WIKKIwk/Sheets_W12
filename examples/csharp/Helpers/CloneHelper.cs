namespace W12CSheets.Client.Helpers;

/// <summary>
/// Object cloning helper
/// </summary>
public static class CloneHelper
{
    /// <summary>
    /// Deep clone using JSON serialization
    /// </summary>
    public static T? DeepClone<T>(T source) where T : class
    {
        if (source == null)
            return null;

        var json = Newtonsoft.Json.JsonConvert.SerializeObject(source);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
    }

    /// <summary>
    /// Shallow clone using MemberwiseClone
    /// </summary>
    public static T? ShallowClone<T>(T source) where T : class
    {
        if (source == null)
            return null;

        var method = source.GetType().GetMethod("MemberwiseClone", 
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        
        return method?.Invoke(source, null) as T;
    }

    /// <summary>
    /// Clone list
    /// </summary>
    public static List<T> CloneList<T>(List<T> source) where T : class
    {
        return source.Select(item => DeepClone(item)!).ToList();
    }

    /// <summary>
    /// Clone dictionary
    /// </summary>
    public static Dictionary<TKey, TValue> CloneDictionary<TKey, TValue>(Dictionary<TKey, TValue> source) 
        where TKey : notnull
        where TValue : class
    {
        return source.ToDictionary(kvp => kvp.Key, kvp => DeepClone(kvp.Value)!);
    }
}
