namespace W12CSheets.Client.Helpers;

/// <summary>
/// Counter helper for tracking counts
/// </summary>
public class Counter
{
    private readonly Dictionary<string, long> _counters = new();
    private readonly object _lock = new();

    public void Increment(string name, long value = 1)
    {
        lock (_lock)
        {
            if (!_counters.ContainsKey(name))
            {
                _counters[name] = 0;
            }
            _counters[name] += value;
        }
    }

    public void Decrement(string name, long value = 1)
    {
        lock (_lock)
        {
            if (!_counters.ContainsKey(name))
            {
                _counters[name] = 0;
            }
            _counters[name] -= value;
        }
    }

    public long Get(string name)
    {
        lock (_lock)
        {
            return _counters.GetValueOrDefault(name, 0);
        }
    }

    public void Set(string name, long value)
    {
        lock (_lock)
        {
            _counters[name] = value;
        }
    }

    public void Reset(string name)
    {
        lock (_lock)
        {
            _counters[name] = 0;
        }
    }

    public void ResetAll()
    {
        lock (_lock)
        {
            _counters.Clear();
        }
    }

    public Dictionary<string, long> GetAll()
    {
        lock (_lock)
        {
            return new Dictionary<string, long>(_counters);
        }
    }
}
