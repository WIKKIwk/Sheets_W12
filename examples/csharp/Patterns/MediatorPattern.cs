namespace W12CSheets.Client.Helpers;

/// <summary>
/// Mediator pattern for managing communication
/// </summary>
public interface IMediator
{
    void Send<T>(T message);
    void Subscribe<T>(Action<T> handler);
    void Unsubscribe<T>(Action<T> handler);
}

public class Mediator : IMediator
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();
    private readonly object _lock = new();

    public void Send<T>(T message)
    {
        List<Delegate> handlers;
        
        lock (_lock)
        {
            var messageType = typeof(T);
            
            if (!_handlers.ContainsKey(messageType))
            {
                return;
            }
            
            handlers = new List<Delegate>(_handlers[messageType]);
        }
        
        foreach (var handler in handlers)
        {
            ((Action<T>)handler)(message!);
        }
    }

    public void Subscribe<T>(Action<T> handler)
    {
        lock (_lock)
        {
            var messageType = typeof(T);
            
            if (!_handlers.ContainsKey(messageType))
            {
                _handlers[messageType] = new List<Delegate>();
            }
            
            _handlers[messageType].Add(handler);
        }
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        lock (_lock)
        {
            var messageType = typeof(T);
            
            if (_handlers.ContainsKey(messageType))
            {
                _handlers[messageType].Remove(handler);
            }
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _handlers.Clear();
        }
    }
}
