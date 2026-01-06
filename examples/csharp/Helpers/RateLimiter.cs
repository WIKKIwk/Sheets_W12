namespace W12CSheets.Client.Helpers;

/// <summary>
/// Rate limiter for controlling request rates
/// </summary>
public class RateLimiter
{
    private readonly int _maxRequests;
    private readonly TimeSpan _timeWindow;
    private readonly Queue<DateTime> _requestTimes = new();
    private readonly object _lock = new();

    public RateLimiter(int maxRequests, TimeSpan timeWindow)
    {
        _maxRequests = maxRequests;
        _timeWindow = timeWindow;
    }

    /// <summary>
    /// Try to acquire permission
    /// </summary>
    public bool TryAcquire()
    {
        lock (_lock)
        {
            var now = DateTime.Now;
            var cutoff = now - _timeWindow;

            // Remove expired timestamps
            while (_requestTimes.Count > 0 && _requestTimes.Peek() < cutoff)
            {
                _requestTimes.Dequeue();
            }

            if (_requestTimes.Count < _maxRequests)
            {
                _requestTimes.Enqueue(now);
                return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Wait until permission is available
    /// </summary>
    public async Task WaitForPermission()
    {
        while (!TryAcquire())
        {
            await Task.Delay(100);
        }
    }

    /// <summary>
    /// Reset limiter
    /// </summary>
    public void Reset()
    {
        lock (_lock)
        {
            _requestTimes.Clear();
        }
    }

    /// <summary>
    /// Get remaining requests
    /// </summary>
    public int RemainingRequests
    {
        get
        {
            lock (_lock)
            {
                return Math.Max(0, _maxRequests - _requestTimes.Count);
            }
        }
    }
}
