namespace W12CSheets.Client.Helpers;

/// <summary>
/// Event bus for decoupled event handling
/// </summary>
public class EventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    private readonly object _lock = new();

    /// <summary>
    /// Subscribe to event
    /// </summary>
    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        lock (_lock)
        {
            var eventType = typeof(TEvent);
            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Delegate>();
            }
            _subscribers[eventType].Add(handler);
        }
    }

    /// <summary>
    /// Unsubscribe from event
    /// </summary>
    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        lock (_lock)
        {
            var eventType = typeof(TEvent);
            if (_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType].Remove(handler);
            }
        }
    }

    /// <summary>
    /// Publish event
    /// </summary>
    public void Publish<TEvent>(TEvent eventData)
    {
        List<Delegate>? handlers;
        lock (_lock)
        {
            var eventType = typeof(TEvent);
            if (!_subscribers.ContainsKey(eventType))
            {
                return;
            }
            handlers = new List<Delegate>(_subscribers[eventType]);
        }

        foreach (var handler in handlers)
        {
            ((Action<TEvent>)handler)(eventData);
        }
    }

    /// <summary>
    /// Publish event async
    /// </summary>
    public async Task PublishAsync<TEvent>(TEvent eventData)
    {
        List<Delegate>? handlers;
        lock (_lock)
        {
            var eventType = typeof(TEvent);
            if (!_subscribers.ContainsKey(eventType))
            {
                return;
            }
            handlers = new List<Delegate>(_subscribers[eventType]);
        }

        var tasks = handlers.Select(handler => Task.Run(() => ((Action<TEvent>)handler)(eventData)));
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Clear all subscriptions
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _subscribers.Clear();
        }
    }
}
