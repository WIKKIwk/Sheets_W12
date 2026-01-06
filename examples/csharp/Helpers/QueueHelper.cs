namespace W12CSheets.Client.Helpers;

/// <summary>
/// Queue helper for managing queues
/// </summary>
public class QueueHelper<T>
{
    private readonly Queue<T> _queue = new();
    private readonly object _lock = new();
    private readonly int _maxSize;

    public QueueHelper(int maxSize = int.MaxValue)
    {
        _maxSize = maxSize;
    }

    /// <summary>
    /// Enqueue item
    /// </summary>
    public void Enqueue(T item)
    {
        lock (_lock)
        {
            if (_queue.Count >= _maxSize)
            {
                _queue.Dequeue(); // Remove oldest if full
            }
            _queue.Enqueue(item);
        }
    }

    /// <summary>
    /// Dequeue item
    /// </summary>
    public T? Dequeue()
    {
        lock (_lock)
        {
            return _queue.Count > 0 ? _queue.Dequeue() : default;
        }
    }

    /// <summary>
    /// Peek at next item
    /// </summary>
    public T? Peek()
    {
        lock (_lock)
        {
            return _queue.Count > 0 ? _queue.Peek() : default;
        }
    }

    /// <summary>
    /// Get queue size
    /// </summary>
    public int Count()
    {
        lock (_lock)
        {
            return _queue.Count;
        }
    }

    /// <summary>
    /// Clear queue
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _queue.Clear();
        }
    }

    /// <summary>
    /// Check if queue is empty
    /// </summary>
    public bool IsEmpty()
    {
        lock (_lock)
        {
            return _queue.Count == 0;
        }
    }

    /// <summary>
    /// Get all items as array
    /// </summary>
    public T[] ToArray()
    {
        lock (_lock)
        {
            return _queue.ToArray();
        }
    }
}
