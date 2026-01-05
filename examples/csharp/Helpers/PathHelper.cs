namespace W12CSheets.Client.Helpers;

/// <summary>
/// Path helper utilities
/// </summary>
public static class PathHelper
{
    /// <summary>
    /// Combine multiple path segments
    /// </summary>
    public static string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    /// <summary>
    /// Get file extension without dot
    /// </summary>
    public static string GetExtensionWithoutDot(string path)
    {
        return Path.GetExtension(path).TrimStart('.');
    }

    /// <summary>
    /// Change file extension
    /// </summary>
    public static string ChangeExtension(string path, string newExtension)
    {
        return Path.ChangeExtension(path, newExtension);
    }

    /// <summary>
    /// Get file name without extension
    /// </summary>
    public static string GetFileNameWithoutExtension(string path)
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    /// <summary>
    /// Ensure path ends with directory separator
    /// </summary>
    public static string EnsureTrailingSlash(string path)
    {
        if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
        {
            return path + Path.DirectorySeparatorChar;
        }
        return path;
    }

    /// <summary>
    /// Get relative path
    /// </summary>
    public static string GetRelativePath(string fromPath, string toPath)
    {
        return Path.GetRelativePath(fromPath, toPath);
    }

    /// <summary>
    /// Normalize path separators
    /// </summary>
    public static string NormalizePath(string path)
    {
        return path.Replace('\\', Path.DirectorySeparatorChar)
                   .Replace('/', Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// Check if path is absolute
    /// </summary>
    public static bool IsAbsolutePath(string path)
    {
        return Path.IsPathRooted(path);
    }

    /// <summary>
    /// Get temp file path
    /// </summary>
    public static string GetTempFilePath(string? extension = null)
    {
        var path = Path.GetTempFileName();
        if (!string.IsNullOrEmpty(extension))
        {
            path = Path.ChangeExtension(path, extension);
        }
        return path;
    }
}
