namespace W12CSheets.Client.Helpers;

/// <summary>
/// Event aggregator for pub/sub pattern
/// </summary>
public class EventAggregator
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    private readonly object _lock = new();

    /// <summary>
    /// Subscribe to event
    /// </summary>
    public void Subscribe<T>(Action<T> handler)
    {
        lock (_lock)
        {
            var eventType = typeof(T);
            
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
    public void Unsubscribe<T>(Action<T> handler)
    {
        lock (_lock)
        {
            var eventType = typeof(T);
            
            if (_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType].Remove(handler);
            }
        }
    }

    /// <summary>
    /// Publish event
    /// </summary>
    public void Publish<T>(T eventData)
    {
        List<Delegate> handlers;
        
        lock (_lock)
        {
            var eventType = typeof(T);
            
            if (!_subscribers.ContainsKey(eventType))
            {
                return;
            }
            
            handlers = new List<Delegate>(_subscribers[eventType]);
        }
        
        foreach (var handler in handlers)
        {
            ((Action<T>)handler)(eventData);
        }
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

    /// <summary>
    /// Get subscriber count for event type
    /// </summary>
    public int GetSubscriberCount<T>()
    {
        lock (_lock)
        {
            var eventType = typeof(T);
            return _subscribers.ContainsKey(eventType) ? _subscribers[eventType].Count : 0;
        }
    }
}
