namespace W12CSheets.Client.Helpers;

/// <summary>
/// Timer and stopwatch helper utilities
/// </summary>
public static class TimerHelper
{
    /// <summary>
    /// Measure execution time of action
    /// </summary>
    public static TimeSpan Measure(Action action)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.Elapsed;
    }

    /// <summary>
    /// Measure execution time of async task
    /// </summary>
    public static async Task<TimeSpan> MeasureAsync(Func<Task> action)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        await action();
        sw.Stop();
        return sw.Elapsed;
    }

    /// <summary>
    /// Create periodic timer
    /// </summary>
    public static System.Threading.Timer CreateTimer(Action action, TimeSpan interval)
    {
        return new System.Threading.Timer(_ => action(), null, interval, interval);
    }

    /// <summary>
    /// Delay with cancellation token
    /// </summary>
    public static async Task Delay(TimeSpan delay, CancellationToken cancellationToken = default)
    {
        await Task.Delay(delay, cancellationToken);
    }

    /// <summary>
    /// Execute action after delay
    /// </summary>
    public static async Task DelayedExecute(Action action, TimeSpan delay)
    {
        await Task.Delay(delay);
        action();
    }

    /// <summary>
    /// Timeout wrapper
    /// </summary>
    public static async Task<T> WithTimeout<T>(Task<T> task, TimeSpan timeout)
    {
        var timeoutTask = Task.Delay(timeout);
        var completedTask = await Task.WhenAny(task, timeoutTask);
        
        if (completedTask == timeoutTask)
        {
            throw new TimeoutException($"Operation timed out after {timeout.TotalSeconds} seconds");
        }
        
        return await task;
    }

    /// <summary>
    /// Format TimeSpan to human readable string
    /// </summary>
    public static string FormatTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.TotalDays >= 1)
            return $"{timeSpan.Days}d {timeSpan.Hours}h {timeSpan.Minutes}m";
        
        if (timeSpan.TotalHours >= 1)
            return $"{timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s";
        
        if (timeSpan.TotalMinutes >= 1)
            return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
        
        if (timeSpan.TotalSeconds >= 1)
            return $"{timeSpan.TotalSeconds:F2}s";
        
        return $"{timeSpan.TotalMilliseconds:F0}ms";
    }
}
