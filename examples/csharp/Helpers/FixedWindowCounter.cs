namespace W12CSheets.Client.Helpers;

/// <summary>
/// Fixed window counter for rate limiting
/// </summary>
public class FixedWindowCounter
{
    private readonly int _limit;
    private readonly TimeSpan _window;
    private int _count;
    private DateTime _windowStart;
    private readonly object _lock = new();

    public FixedWindowCounter(int limit, TimeSpan window)
    {
        _limit = limit;
        _window = window;
        _count = 0;
        _windowStart = DateTime.Now;
    }

    public bool TryIncrement()
    {
        lock (_lock)
        {
            var now = DateTime.Now;

            if (now - _windowStart >= _window)
            {
                _count = 0;
                _windowStart = now;
            }

            if (_count < _limit)
            {
                _count++;
                return true;
            }

            return false;
        }
    }

    public int CurrentCount
    {
        get
        {
            lock (_lock)
            {
                if (DateTime.Now - _windowStart >= _window)
                {
                    return 0;
                }
                return _count;
            }
        }
    }

    public TimeSpan TimeUntilReset
    {
        get
        {
            lock (_lock)
            {
                var elapsed = DateTime.Now - _windowStart;
                return elapsed >= _window ? TimeSpan.Zero : _window - elapsed;
            }
        }
    }
}
