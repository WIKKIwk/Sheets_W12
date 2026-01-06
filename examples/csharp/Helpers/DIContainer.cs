namespace W12CSheets.Client.Helpers;

/// <summary>
/// Dependency injection container
/// </summary>
public class DIContainer
{
    private readonly Dictionary<Type, Func<object>> _registrations = new();
    private readonly Dictionary<Type, object> _singletons = new();
    private readonly object _lock = new();

    /// <summary>
    /// Register transient service
    /// </summary>
    public void RegisterTransient<TInterface, TImplementation>() 
        where TImplementation : TInterface, new()
    {
        lock (_lock)
        {
            _registrations[typeof(TInterface)] = () => new TImplementation();
        }
    }

    /// <summary>
    /// Register singleton service
    /// </summary>
    public void RegisterSingleton<TInterface, TImplementation>() 
        where TImplementation : TInterface, new()
    {
        lock (_lock)
        {
            _registrations[typeof(TInterface)] = () =>
            {
                var type = typeof(TInterface);
                if (!_singletons.ContainsKey(type))
                {
                    _singletons[type] = new TImplementation();
                }
                return _singletons[type];
            };
        }
    }

    /// <summary>
    /// Register instance
    /// </summary>
    public void RegisterInstance<TInterface>(TInterface instance) where TInterface : class
    {
        lock (_lock)
        {
            _singletons[typeof(TInterface)] = instance;
            _registrations[typeof(TInterface)] = () => instance;
        }
    }

    /// <summary>
    /// Resolve service
    /// </summary>
    public T? Resolve<T>()
    {
        lock (_lock)
        {
            var type = typeof(T);
            
            if (_registrations.TryGetValue(type, out var factory))
            {
                return (T)factory();
            }
            
            return default;
        }
    }

    /// <summary>
    /// Check if service is registered
    /// </summary>
    public bool IsRegistered<T>()
    {
        lock (_lock)
        {
            return _registrations.ContainsKey(typeof(T));
        }
    }

    /// <summary>
    /// Clear all registrations
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _registrations.Clear();
            _singletons.Clear();
        }
    }
}
