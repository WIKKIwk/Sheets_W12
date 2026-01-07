namespace W12CSheets.Client.Helpers;

/// <summary>
/// Message queue for async message processing
/// </summary>
public class MessageQueue<T>
{
    private readonly Queue<T> _queue = new();
    private readonly SemaphoreSlim _semaphore = new(0);
    private readonly object _lock = new();

    public void Enqueue(T message)
    {
        lock (_lock)
        {
            _queue.Enqueue(message);
        }
        _semaphore.Release();
    }

    public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        lock (_lock)
        {
            return _queue.Dequeue();
        }
    }

    public bool TryDequeue(out T? message)
    {
        lock (_lock)
        {
            if (_queue.Count > 0)
            {
                message = _queue.Dequeue();
                return true;
            }
            message = default;
            return false;
        }
    }

    public int Count
    {
        get
        {
            lock(_lock)
            {
                return _queue.Count;
            }
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _queue.Clear();
        }
    }
}
