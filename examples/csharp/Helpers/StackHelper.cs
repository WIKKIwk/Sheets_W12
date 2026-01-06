namespace W12CSheets.Client.Helpers;

/// <summary>
/// Stack helper for managing stacks
/// </summary>
public class StackHelper<T>
{
    private readonly Stack<T> _stack = new();
    private readonly object _lock = new();
    private readonly int _maxSize;

    public StackHelper(int maxSize = int.MaxValue)
    {
        _maxSize = maxSize;
    }

    /// <summary>
    /// Push item onto stack
    /// </summary>
    public void Push(T item)
    {
        lock (_lock)
        {
            if (_stack.Count >= _maxSize)
            {
                throw new InvalidOperationException("Stack is full");
            }
            _stack.Push(item);
        }
    }

    /// <summary>
    /// Pop item from stack
    /// </summary>
    public T? Pop()
    {
        lock (_lock)
        {
            return _stack.Count > 0 ? _stack.Pop() : default;
        }
    }

    /// <summary>
    /// Peek at top item
    /// </summary>
    public T? Peek()
    {
        lock (_lock)
        {
            return _stack.Count > 0 ? _stack.Peek() : default;
        }
    }

    /// <summary>
    /// Get stack size
    /// </summary>
    public int Count()
    {
        lock (_lock)
        {
            return _stack.Count;
        }
    }

    /// <summary>
    /// Clear stack
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _stack.Clear();
        }
    }

    /// <summary>
    /// Check if stack is empty
    /// </summary>
    public bool IsEmpty()
    {
        lock (_lock)
        {
            return _stack.Count == 0;
        }
    }

    /// <summary>
    /// Get all items as array
    /// </summary>
    public T[] ToArray()
    {
        lock (_lock)
        {
            return _stack.ToArray();
        }
    }
}
