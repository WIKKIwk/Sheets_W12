namespace W12CSheets.Client.Helpers;

/// <summary>
/// Lazy loading helper
/// </summary>
public class LazyHelper<T> where T : class
{
    private T? _value;
    private readonly Func<T> _valueFactory;
    private readonly object _lock = new();
    private bool _isValueCreated;

    public LazyHelper(Func<T> valueFactory)
    {
        _valueFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory));
    }

    /// <summary>
    /// Get the lazy-loaded value
    /// </summary>
    public T Value
    {
        get
        {
            if (!_isValueCreated)
            {
                lock (_lock)
                {
                    if (!_isValueCreated)
                    {
                        _value = _valueFactory();
                        _isValueCreated = true;
                    }
                }
            }
            
            return _value!;
        }
    }

    /// <summary>
    /// Check if value has been created
    /// </summary>
    public bool IsValueCreated => _isValueCreated;

    /// <summary>
    /// Reset the lazy value
    /// </summary>
    public void Reset()
    {
        lock (_lock)
        {
            _value = null;
            _isValueCreated = false;
        }
    }
}
