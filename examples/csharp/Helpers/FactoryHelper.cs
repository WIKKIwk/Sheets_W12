namespace W12CSheets.Client.Helpers;

/// <summary>
/// Factory pattern helper
/// </summary>
public class FactoryHelper<TKey, TValue> where TKey : notnull where TValue : class
{
    private readonly Dictionary<TKey, Func<TValue>> _factories = new();

    /// <summary>
    /// Register factory function
    /// </summary>
    public void Register(TKey key, Func<TValue> factory)
    {
        _factories[key] = factory;
    }

    /// <summary>
    /// Create instance using registered factory
    /// </summary>
    public TValue? Create(TKey key)
    {
        if (_factories.TryGetValue(key, out var factory))
        {
            return factory();
        }
        
        return null;
    }

    /// <summary>
    /// Check if factory is registered
    /// </summary>
    public bool IsRegistered(TKey key)
    {
        return _factories.ContainsKey(key);
    }

    /// <summary>
    /// Unregister factory
    /// </summary>
    public void Unregister(TKey key)
    {
        _factories.Remove(key);
    }

    /// <summary>
    /// Clear all factories
    /// </summary>
    public void Clear()
    {
        _factories.Clear();
    }

    /// <summary>
    /// Get all registered keys
    /// </summary>
    public IEnumerable<TKey> GetRegisteredKeys()
    {
        return _factories.Keys;
    }
}
