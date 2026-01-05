using System.Text;

namespace W12CSheets.Client.Helpers;

/// <summary>
/// File helper utilities
/// </summary>
public static class FileHelper
{
    /// <summary>
    /// Get file size in human readable format
    /// </summary>
    public static string GetFileSizeString(long bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len /= 1024;
        }

        return $"{len:0.##} {sizes[order]}";
    }

    /// <summary>
    /// Ensure directory exists
    /// </summary>
    public static void EnsureDirectoryExists(string path)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// Get unique file name if file already exists
    /// </summary>
    public static string GetUniqueFileName(string filePath)
    {
        if (!File.Exists(filePath))
            return filePath;

        var directory = Path.GetDirectoryName(filePath) ?? "";
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var extension = Path.GetExtension(filePath);

        int counter = 1;
        string newPath;

        do
        {
            newPath = Path.Combine(directory, $"{fileName} ({counter}){extension}");
            counter++;
        }
        while (File.Exists(newPath));

        return newPath;
    }

    /// <summary>
    /// Read all text with encoding detection
    /// </summary>
    public static string ReadAllTextWithEncoding(string filePath)
    {
        using var reader = new StreamReader(filePath, true);
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Write all text safely (with backup)
    /// </summary>
    public static void WriteAllTextSafely(string filePath, string content)
    {
        string? backupPath = null;

        try
        {
            // Create backup if file exists
            if (File.Exists(filePath))
            {
                backupPath = filePath + ".backup";
                File.Copy(filePath, backupPath, true);
            }

            // Write new content
            File.WriteAllText(filePath, content);

            // Delete backup on success
            if (backupPath != null && File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }
        }
        catch
        {
            // Restore backup on failure
            if (backupPath != null && File.Exists(backupPath))
            {
                File.Copy(backupPath, filePath, true);
                File.Delete(backupPath);
            }
            throw;
        }
    }

    /// <summary>
    /// Get files in directory recursively
    /// </summary>
    public static IEnumerable<string> GetFilesRecursive(string directory, string searchPattern = "*.*")
    {
        var files = new List<string>();

        try
        {
            files.AddRange(Directory.GetFiles(directory, searchPattern));

            foreach (var subdirectory in Directory.GetDirectories(directory))
            {
                files.AddRange(GetFilesRecursive(subdirectory, searchPattern));
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip directories we don't have access to
        }

        return files;
    }

    /// <summary>
    /// Delete directory with retry
    /// </summary>
    public static void DeleteDirectoryWithRetry(string directory, int maxRetries = 3)
    {
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
                return;
            }
            catch when (i < maxRetries - 1)
            {
                Thread.Sleep(100);
            }
        }
    }

    /// <summary>
    /// Copy directory recursively
    /// </summary>
    public static void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
    {
        var dir = new DirectoryInfo(sourceDir);

        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

        Directory.CreateDirectory(destDir);

        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destDir, file.Name);
            file.CopyTo(targetFilePath, true);
        }

        if (recursive)
        {
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                string newDestinationDir = Path.Combine(destDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }
}
