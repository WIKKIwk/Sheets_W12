namespace W12CSheets.Client.Helpers;

/// <summary>
/// Object pool for reusing expensive objects
/// </summary>
public class ObjectPool<T> where T : class, new()
{
    private readonly Stack<T> _pool = new();
    private readonly int _maxSize;
    private readonly object _lock = new();
    private int _currentSize;

    public ObjectPool(int maxSize = 100)
    {
        _maxSize = maxSize;
    }

    /// <summary>
    /// Get object from pool
    /// </summary>
    public T Get()
    {
        lock (_lock)
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop();
            }

            _currentSize++;
            return new T();
        }
    }

    /// <summary>
    /// Return object to pool
    /// </summary>
    public void Return(T item)
    {
        lock (_lock)
        {
            if (_pool.Count < _maxSize)
            {
                _pool.Push(item);
            }
            else
            {
                _currentSize--;
            }
        }
    }

    /// <summary>
    /// Clear pool
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _pool.Clear();
            _currentSize = 0;
        }
    }

    /// <summary>
    /// Get pool statistics
    /// </summary>
    public (int Available, int Total) GetStatistics()
    {
        lock (_lock)
        {
            return (_pool.Count, _currentSize);
        }
    }
}
