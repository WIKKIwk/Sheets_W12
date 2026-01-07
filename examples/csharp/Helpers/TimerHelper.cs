namespace W12CSheets.Client.Helpers;

/// <summary>
/// Timer helper for scheduling tasks
/// </summary>
public class TimerHelper
{
    private System.Threading.Timer? _timer;
    private readonly object _lock = new();

    /// <summary>
    /// Schedule action to run after delay
    /// </summary>
    public void Schedule(Action action, TimeSpan delay)
    {
        lock (_lock)
        {
            _timer?.Dispose();
            _timer = new System.Threading.Timer(_ => action(), null, delay, Timeout.InfiniteTimeSpan);
        }
    }

    /// <summary>
    /// Schedule recurring action
    /// </summary>
    public void ScheduleRecurring(Action action, TimeSpan interval)
    {
        lock (_lock)
        {
            _timer?.Dispose();
            _timer = new System.Threading.Timer(_ => action(), null, TimeSpan.Zero, interval);
        }
    }

    /// <summary>
    /// Cancel scheduled action
    /// </summary>
    public void Cancel()
    {
        lock (_lock)
        {
            _timer?.Dispose();
            _timer = null;
        }
    }

    /// <summary>
    /// Dispose timer
    /// </summary>
    public void Dispose()
    {
        Cancel();
    }
}
