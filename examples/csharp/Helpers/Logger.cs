namespace W12CSheets.Client.Helpers;

/// <summary>
/// Logger helper for simple logging
/// </summary>
public static class Logger
{
    private static readonly List<string> _logs = new();
    private static readonly object _lock = new();
    public static LogLevel MinimumLevel { get; set; } = LogLevel.Info;

    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public static void Log(LogLevel level, string message)
    {
        if (level < MinimumLevel) return;

        var logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
        
        lock (_lock)
        {
            _logs.Add(logEntry);
            Console.WriteLine(logEntry);
        }
    }

    public static void Trace(string message) => Log(LogLevel.Trace, message);
    public static void Debug(string message) => Log(LogLevel.Debug, message);
    public static void Info(string message) => Log(LogLevel.Info, message);
    public static void Warning(string message) => Log(LogLevel.Warning, message);
    public static void Error(string message) => Log(LogLevel.Error, message);
    public static void Fatal(string message) => Log(LogLevel.Fatal, message);

    public static List<string> GetLogs()
    {
        lock (_lock)
        {
            return new List<string>(_logs);
        }
    }

    public static void Clear()
    {
        lock (_lock)
        {
            _logs.Clear();
        }
    }
}
