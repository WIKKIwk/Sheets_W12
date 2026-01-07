namespace W12CSheets.Client.Helpers;

/// <summary>
/// Progress tracker for long-running operations
/// </summary>
public class ProgressTracker
{
    private readonly int _total;
    private int _current;
    private readonly object _lock = new();
    private DateTime _startTime;

    public event Action<int, int>? OnProgress;

    public ProgressTracker(int total)
    {
        _total = total;
        _current = 0;
        _startTime = DateTime.Now;
    }

    public void Increment(int count = 1)
    {
        lock (_lock)
        {
            _current += count;
            OnProgress?.Invoke(_current, _total);
        }
    }

    public int PercentComplete
    {
        get
        {
            lock (_lock)
            {
                return _total > 0 ? (_current * 100) / _total : 0;
            }
        }
    }

    public TimeSpan ElapsedTime => DateTime.Now - _startTime;

    public TimeSpan EstimatedTimeRemaining
    {
        get
        {
            lock (_lock)
            {
                if (_current == 0) return TimeSpan.Zero;
                var avgTimePerItem = ElapsedTime.TotalSeconds / _current;
                var remaining = _total - _current;
                return TimeSpan.FromSeconds(avgTimePerItem * remaining);
            }
        }
    }

    public bool IsComplete => _current >= _total;

    public void Reset()
    {
        lock (_lock)
        {
            _current = 0;
            _startTime = DateTime.Now;
        }
    }
}
