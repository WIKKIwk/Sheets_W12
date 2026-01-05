namespace W12CSheets.Client.Helpers;

/// <summary>
/// Process helper utilities
/// </summary>
public static class ProcessHelper
{
    /// <summary>
    /// Execute command and get output
    /// </summary>
    public static string Execute(string fileName, string arguments = "")
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        
        return output;
    }

    /// <summary>
    /// Execute command asynchronously
    /// </summary>
    public static async Task<string> ExecuteAsync(string fileName, string arguments = "")
    {
        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        
        process.Start();
        string output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        
        return output;
    }

    /// <summary>
    /// Check if process is running
    /// </summary>
    public static bool IsProcessRunning(string processName)
    {
        return System.Diagnostics.Process.GetProcessesByName(processName).Length > 0;
    }

    /// <summary>
    /// Kill process by name
    /// </summary>
    public static void KillProcess(string processName)
    {
        var processes = System.Diagnostics.Process.GetProcessesByName(processName);
        foreach (var process in processes)
        {
            process.Kill();
        }
    }

    /// <summary>
    /// Get current process memory usage in MB
    /// </summary>
    public static double GetMemoryUsageMB()
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();
        return process.WorkingSet64 / 1024.0 / 1024.0;
    }

    /// <summary>
    /// Open URL in default browser
    /// </summary>
    public static void OpenUrl(string url)
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    /// <summary>
    /// Open file in default application
    /// </summary>
    public static void OpenFile(string filePath)
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }
}
