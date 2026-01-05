namespace W12CSheets.Client.Helpers;

/// <summary>
/// System information helper
/// </summary>
public static class SystemInfo
{
    /// <summary>
    /// Get operating system information
    /// </summary>
    public static string GetOSInfo()
    {
        return Environment.OSVersion.ToString();
    }

    /// <summary>
    /// Get.NET runtime version
    /// </summary>
    public static string GetRuntimeVersion()
    {
        return Environment.Version.ToString();
    }

    /// <summary>
    /// Get computer name
    /// </summary>
    public static string GetComputerName()
    {
        return Environment.MachineName;
    }

    /// <summary>
    /// Get username
    /// </summary>
    public static string GetUsername()
    {
        return Environment.UserName;
    }

    /// <summary>
    /// Get processor count
    /// </summary>
    public static int GetProcessorCount()
    {
        return Environment.ProcessorCount;
    }

    /// <summary>
    /// Get system uptime
    /// </summary>
    public static TimeSpan GetUptime()
    {
        return TimeSpan.FromMilliseconds(Environment.TickCount64);
    }

    /// <summary>
    /// Get current directory
    /// </summary>
    public static string GetCurrentDirectory()
    {
        return Environment.CurrentDirectory;
    }

    /// <summary>
    /// Get user's home directory
    /// </summary>
    public static string GetHomeDirectory()
    {
        return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }

    /// <summary>
    /// Check if 64-bit operating system
    /// </summary>
    public static bool Is64BitOS()
    {
        return Environment.Is64BitOperatingSystem;
    }

    /// <summary>
    /// Check if 64-bit process
    /// </summary>
    public static bool Is64BitProcess()
    {
        return Environment.Is64BitProcess;
    }

    /// <summary>
    /// Get all environment variables
    /// </summary>
    public static Dictionary<string, string> GetEnvironmentVariables()
    {
        var vars = new Dictionary<string, string>();
        foreach (System.Collections.DictionaryEntry entry in Environment.GetEnvironmentVariables())
        {
            vars[entry.Key.ToString()!] = entry.Value?.ToString() ?? "";
        }
        return vars;
    }

    /// <summary>
    /// Get system summary
    /// </summary>
    public static string GetSystemSummary()
    {
        return $@"System Information:
OS: {GetOSInfo()}
Runtime: {GetRuntimeVersion()}
Computer: {GetComputerName()}
User: {GetUsername()}
Processors: {GetProcessorCount()}
Uptime: {GetUptime()}
64-bit OS: {Is64BitOS()}
64-bit Process: {Is64BitProcess()}";
    }
}
