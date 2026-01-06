namespace W12CSheets.Client.Helpers;

/// <summary>
/// Throttle helper for limiting execution frequency
/// </summary>
public class Throttle
{
    private DateTime _lastExecutionTime = DateTime.MinValue;
    private readonly TimeSpan _minInterval;
    private readonly object _lock = new();

    public Throttle(TimeSpan minInterval)
    {
        _minInterval = minInterval;
    }

    /// <summary>
    /// Execute action with throttling
    /// </summary>
    public void Execute(Action action)
    {
        lock (_lock)
        {
            var now = DateTime.Now;
            var timeSinceLastExecution = now - _lastExecutionTime;

            if (timeSinceLastExecution >= _minInterval)
            {
                action();
                _lastExecutionTime = now;
            }
        }
    }

    /// <summary>
    /// Execute async action with throttling
    /// </summary>
    public async Task ExecuteAsync(Func<Task> action)
    {
        DateTime? executionTime = null;

        lock (_lock)
        {
            var now = DateTime.Now;
            var timeSinceLastExecution = now - _lastExecutionTime;

            if (timeSinceLastExecution >= _minInterval)
            {
                executionTime = now;
            }
        }

        if (executionTime.HasValue)
        {
            await action();
            lock (_lock)
            {
                _lastExecutionTime = executionTime.Value;
            }
        }
    }

    /// <summary>
    /// Reset throttle
    /// </summary>
    public void Reset()
    {
        lock (_lock)
        {
            _lastExecutionTime = DateTime.MinValue;
        }
    }

    /// <summary>
    /// Check if can execute now
    /// </summary>
    public bool CanExecute
    {
        get
        {
            lock (_lock)
            {
                return (DateTime.Now - _lastExecutionTime) >= _minInterval;
            }
        }
    }
}
