namespace W12CSheets.Client.Helpers;

/// <summary>
/// Serialization helper utilities
/// </summary>
public static class SerializationHelper
{
    /// <summary>
    /// Serialize object to JSON
    /// </summary>
    public static string ToJson<T>(T obj, bool indented = false)
    {
        var settings = new Newtonsoft.Json.JsonSerializerSettings
        {
            Formatting = indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
        };
        
        return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);
    }

    /// <summary>
    /// Deserialize JSON to object
    /// </summary>
    public static T? FromJson<T>(string json)
    {
        try
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
            return default;
        }
    }

    /// <summary>
    /// Serialize to binary
    /// </summary>
    public static byte[] ToBinary<T>(T obj)
    {
        using var ms = new System.IO.MemoryStream();
        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        #pragma warning disable SYSLIB0011
        formatter.Serialize(ms, obj!);
        #pragma warning restore SYSLIB0011
        return ms.ToArray();
    }

    /// <summary>
    /// Deserialize from binary
    /// </summary>
    public static T? FromBinary<T>(byte[] data)
    {
        try
        {
            using var ms = new System.IO.MemoryStream(data);
            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            #pragma warning disable SYSLIB0011
            return (T?)formatter.Deserialize(ms);
            #pragma warning restore SYSLIB0011
        }
        catch
        {
            return default;
        }
    }
}
