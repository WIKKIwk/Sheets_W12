namespace W12CSheets.Client.Helpers;

/// <summary>
/// Debounce helper for delaying execution
/// </summary>
public class Debounce
{
    private Timer? _timer;
    private readonly TimeSpan _delay;
    private readonly object _lock = new();

    public Debounce(TimeSpan delay)
    {
        _delay = delay;
    }

    /// <summary>
    /// Execute action after delay, resetting timer on each call
    /// </summary>
    public void Execute(Action action)
    {
        lock (_lock)
        {
            _timer?.Dispose();
            _timer = new Timer(_ => action(), null, _delay, Timeout.InfiniteTimeSpan);
        }
    }

    /// <summary>
    /// Cancel pending execution
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
    /// Dispose resources
    /// </summary>
    public void Dispose()
    {
        Cancel();
    }
}
