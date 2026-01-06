namespace W12CSheets.Client.Helpers;

/// <summary>
/// Stream helper utilities
/// </summary>
public static class StreamHelper
{
    /// <summary>
    /// Read all bytes from stream
    /// </summary>
    public static byte[] ReadAllBytes(Stream stream)
    {
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Read all bytes async
    /// </summary>
    public static async Task<byte[]> ReadAllBytesAsync(Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        return ms.ToArray();
    }

    /// <summary>
    /// Read all text from stream
    /// </summary>
    public static string ReadAllText(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Read all text async
    /// </summary>
    public static async Task<string> ReadAllTextAsync(Stream stream)
    {
        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync();
    }

    /// <summary>
    /// Write text to stream
    /// </summary>
    public static void WriteText(Stream stream, string text)
    {
        using var writer = new StreamWriter(stream);
        writer.Write(text);
        writer.Flush();
    }

    /// <summary>
    /// Write text to stream async
    /// </summary>
    public static async Task WriteTextAsync(Stream stream, string text)
    {
        using var writer = new StreamWriter(stream);
        await writer.WriteAsync(text);
        await writer.FlushAsync();
    }

    /// <summary>
    /// Copy stream to another stream
    /// </summary>
    public static void CopyStream(Stream source, Stream destination)
    {
        source.CopyTo(destination);
    }

    /// <summary>
    /// Copy stream async
    /// </summary>
    public static async Task CopyStreamAsync(Stream source, Stream destination)
    {
        await source.CopyToAsync(destination);
    }

    /// <summary>
    /// Convert string to stream
    /// </summary>
    public static Stream StringToStream(string text)
    {
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Write(text);
        writer.Flush();
        ms.Position = 0;
        return ms;
    }
}
