namespace W12CSheets.Client.Helpers;

/// <summary>
/// Token bucket for rate limiting
/// </summary>
public class TokenBucket
{
    private readonly int _capacity;
    private readonly double _refillRate;
    private double _tokens;
    private DateTime _lastRefill;
    private readonly object _lock = new();

    public TokenBucket(int capacity, double refillRate)
    {
        _capacity = capacity;
        _refillRate = refillRate;
        _tokens = capacity;
        _lastRefill = DateTime.Now;
    }

    public bool TryConsume(int tokens = 1)
    {
        lock (_lock)
        {
            Refill();

            if (_tokens >= tokens)
            {
                _tokens -= tokens;
                return true;
            }

            return false;
        }
    }

    public async Task<bool> ConsumeAsync(int tokens = 1)
    {
        while (true)
        {
            if (TryConsume(tokens))
            {
                return true;
            }

            await Task.Delay(100);
        }
    }

    private void Refill()
    {
        var now = DateTime.Now;
        var elapsed = (now - _lastRefill).TotalSeconds;
        var tokensToAdd = elapsed * _refillRate;

        _tokens = Math.Min(_capacity, _tokens + tokensToAdd);
        _lastRefill = now;
    }

    public double AvailableTokens
    {
        get
        {
            lock (_lock)
            {
                Refill();
                return _tokens;
            }
        }
    }
}
