namespace W12CSheets.Client.Helpers;

/// <summary>
/// Observer pattern helper
/// </summary>
public interface IObserver<T>
{
    void Update(T data);
}

public interface IObservable<T>
{
    void Attach(IObserver<T> observer);
    void Detach(IObserver<T> observer);
    void Notify(T data);
}

public class ObservableSubject<T> : IObservable<T>
{
    private readonly List<IObserver<T>> _observers = new();
    private readonly object _lock = new();

    public void Attach(IObserver<T> observer)
    {
        lock (_lock)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }
    }

    public void Detach(IObserver<T> observer)
    {
        lock (_lock)
        {
            _observers.Remove(observer);
        }
    }

    public void Notify(T data)
    {
        List<IObserver<T>> observersCopy;
        
        lock (_lock)
        {
            observersCopy = new List<IObserver<T>>(_observers);
        }
        
        foreach (var observer in observersCopy)
        {
            observer.Update(data);
        }
    }

    public int ObserverCount
    {
        get
        {
            lock (_lock)
            {
                return _observers.Count;
            }
        }
    }

    public void ClearObservers()
    {
        lock (_lock)
        {
            _observers.Clear();
        }
    }
}
