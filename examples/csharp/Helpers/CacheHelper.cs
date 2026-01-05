namespace W12CSheets.Client.Helpers;

/// <summary>
/// Cache helper utilities
/// </summary>
public class CacheHelper<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, CacheItem<TValue>> _cache = new();
    private readonly TimeSpan _defaultExpiration;
    private readonly object _lock = new();

    public CacheHelper(TimeSpan? defaultExpiration = null)
    {
        _defaultExpiration = defaultExpiration ?? TimeSpan.FromMinutes(10);
    }

    /// <summary>
    /// Add or update item in cache
    /// </summary>
    public void Set(TKey key, TValue value, TimeSpan? expiration = null)
    {
        lock (_lock)
        {
            var expiresAt = DateTime.Now + (expiration ?? _defaultExpiration);
            _cache[key] = new CacheItem<TValue>(value, expiresAt);
        }
    }

    /// <summary>
    /// Get item from cache
    /// </summary>
    public TValue? Get(TKey key)
    {
        lock (_lock)
        {
            if (!_cache.TryGetValue(key, out var item))
                return default;

            if (item.IsExpired)
            {
                _cache.Remove(key);
                return default;
            }

            return item.Value;
        }
    }

    /// <summary>
    /// Get or add item to cache
    /// </summary>
    public TValue GetOrAdd(TKey key, Func<TValue> valueFactory, TimeSpan? expiration = null)
    {
        var value = Get(key);
        
        if (value != null)
            return value;

        value = valueFactory();
        Set(key, value, expiration);
        return value;
    }

    /// <summary>
    /// Remove item from cache
    /// </summary>
    public void Remove(TKey key)
    {
        lock (_lock)
        {
            _cache.Remove(key);
        }
    }

    /// <summary>
    /// Clear all cache
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _cache.Clear();
        }
    }

    /// <summary>
    /// Remove expired items
    /// </summary>
    public void RemoveExpired()
    {
        lock (_lock)
        {
            var expiredKeys = _cache
                .Where(kvp => kvp.Value.IsExpired)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.Remove(key);
            }
        }
    }

    /// <summary>
    /// Get cache statistics
    /// </summary>
    public CacheStatistics GetStatistics()
    {
        lock (_lock)
        {
            var total = _cache.Count;
            var expired = _cache.Count(kvp => kvp.Value.IsExpired);
            
            return new CacheStatistics
            {
                TotalItems = total,
                ExpiredItems = expired,
                ActiveItems = total - expired
            };
        }
    }
}

public class CacheItem<T>
{
    public T Value { get; }
    public DateTime ExpiresAt { get; }
    public bool IsExpired => DateTime.Now >= ExpiresAt;

    public CacheItem(T value, DateTime expiresAt)
    {
        Value = value;
        ExpiresAt = expiresAt;
    }
}

public class CacheStatistics
{
    public int TotalItems { get; set; }
    public int ExpiredItems { get; set; }
    public int ActiveItems { get; set; }
}
