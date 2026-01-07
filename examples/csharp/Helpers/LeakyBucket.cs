namespace W12CSheets.Client.Helpers;

/// <summary>
/// Leaky bucket for rate limiting
/// </summary>
public class LeakyBucket
{
    private readonly int _capacity;
    private readonly int _leakRate;
    private int _level;
    private DateTime _lastLeak;
    private readonly object _lock = new();

    public LeakyBucket(int capacity, int leakRate)
    {
        _capacity = capacity;
        _leakRate = leakRate;
        _level = 0;
        _lastLeak = DateTime.Now;
    }

    public bool TryAdd(int amount = 1)
    {
        lock (_lock)
        {
            Leak();

            if (_level + amount <= _capacity)
            {
                _level += amount;
                return true;
            }

            return false;
        }
    }

    private void Leak()
    {
        var now = DateTime.Now;
        var elapsed = (now - _lastLeak).TotalSeconds;
        var leaked = (int)(elapsed * _leakRate);

        _level = Math.Max(0, _level - leaked);
        _lastLeak = now;
    }

    public int CurrentLevel
    {
        get
        {
            lock (_lock)
            {
                Leak();
                return _level;
            }
        }
    }

    public int AvailableCapacity
    {
        get
        {
            lock (_lock)
            {
                Leak();
                return _capacity - _level;
            }
        }
    }
}
