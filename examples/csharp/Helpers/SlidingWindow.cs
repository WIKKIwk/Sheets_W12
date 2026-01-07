namespace W12CSheets.Client.Helpers;

/// <summary>
/// Sliding window for rate limiting
/// </summary>
public class SlidingWindow
{
    private readonly int _windowSize;
    private readonly TimeSpan _windowDuration;
    private readonly Queue<DateTime> _timestamps = new();
    private readonly object _lock = new();

    public SlidingWindow(int windowSize, TimeSpan windowDuration)
    {
        _windowSize = windowSize;
        _windowDuration = windowDuration;
    }

    public bool TryRecord()
    {
        lock (_lock)
        {
            var now = DateTime.Now;
            var cutoff = now - _windowDuration;

            while (_timestamps.Count > 0 && _timestamps.Peek() < cutoff)
            {
                _timestamps.Dequeue();
            }

            if (_timestamps.Count < _windowSize)
            {
                _timestamps.Enqueue(now);
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
                var now = DateTime.Now;
                var cutoff = now - _windowDuration;

                while (_timestamps.Count > 0 && _timestamps.Peek() < cutoff)
                {
                    _timestamps.Dequeue();
                }

                return _timestamps.Count;
            }
        }
    }

    public void Reset()
    {
        lock (_lock)
        {
            _timestamps.Clear();
        }
    }
}
