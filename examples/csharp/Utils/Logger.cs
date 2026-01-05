namespace W12CSheets.Client.Utils;

/// <summary>
/// Simple logger for application events
/// </summary>
public class Logger
{
    private readonly string _logFilePath;
    private readonly bool _enableConsoleOutput;
    private readonly LogLevel _minimumLevel;

    public Logger(string logFilePath = "app.log", bool enableConsoleOutput = true, LogLevel minimumLevel = LogLevel.Info)
    {
        _logFilePath = logFilePath;
        _enableConsoleOutput = enableConsoleOutput;
        _minimumLevel = minimumLevel;
    }

    public void Debug(string message)
    {
        Log(LogLevel.Debug, message);
    }

    public void Info(string message)
    {
        Log(LogLevel.Info, message);
    }

    public void Warning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    public void Error(string message, Exception? exception = null)
    {
        var fullMessage = exception != null
            ? $"{message}\nException: {exception.Message}\nStackTrace: {exception.StackTrace}"
            : message;
        
        Log(LogLevel.Error, fullMessage);
    }

    private void Log(LogLevel level, string message)
    {
        if (level < _minimumLevel)
            return;

        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logEntry = $"[{timestamp}] [{level}] {message}";

        // Console output
        if (_enableConsoleOutput)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = GetColorForLevel(level);
            Console.WriteLine(logEntry);
            Console.ForegroundColor = originalColor;
        }

        // File output
        try
        {
            File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
        }
        catch
        {
            // Silently fail if can't write to log file
        }
    }

    private ConsoleColor GetColorForLevel(LogLevel level)
    {
        return level switch
        {
            LogLevel.Debug => ConsoleColor.Gray,
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }

    public void ClearLog()
    {
        try
        {
            File.WriteAllText(_logFilePath, string.Empty);
        }
        catch
        {
            // Silently fail
        }
    }
}

public enum LogLevel
{
    Debug = 0,
    Info = 1,
    Warning = 2,
    Error = 3
}
