namespace W12CSheets.Client.Helpers;

/// <summary>
/// Compression helper utilities
/// </summary>
public static class CompressionHelper
{
    /// <summary>
    /// Compress string using GZip
    /// </summary>
    public static byte[] Compress(string text)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(text);
        
        using var output = new System.IO.MemoryStream();
        using (var gzip = new System.IO.Compression.GZipStream(output, System.IO.Compression.CompressionMode.Compress))
        {
            gzip.Write(bytes, 0, bytes.Length);
        }
        
        return output.ToArray();
    }

    /// <summary>
    /// Decompress GZip bytes to string
    /// </summary>
    public static string Decompress(byte[] compressedBytes)
    {
        using var input = new System.IO.MemoryStream(compressedBytes);
        using var gzip = new System.IO.Compression.GZipStream(input, System.IO.Compression.CompressionMode.Decompress);
        using var output = new System.IO.MemoryStream();
        
        gzip.CopyTo(output);
        return System.Text.Encoding.UTF8.GetString(output.ToArray());
    }

    /// <summary>
    /// Compress file
    /// </summary>
    public static void CompressFile(string sourcePath, string destinationPath)
    {
        using var sourceFile = File.OpenRead(sourcePath);
        using var destinationFile = File.Create(destinationPath);
        using var gzip = new System.IO.Compression.GZipStream(destinationFile, System.IO.Compression.CompressionMode.Compress);
        
        sourceFile.CopyTo(gzip);
    }

    /// <summary>
    /// Decompress file
    /// </summary>
    public static void DecompressFile(string sourcePath, string destinationPath)
    {
        using var sourceFile = File.OpenRead(sourcePath);
        using var gzip = new System.IO.Compression.GZipStream(sourceFile, System.IO.Compression.CompressionMode.Decompress);
        using var destinationFile = File.Create(destinationPath);
        
        gzip.CopyTo(destinationFile);
    }
}
