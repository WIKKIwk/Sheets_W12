namespace W12CSheets.Client.Patterns;

/// <summary>
/// Memento pattern for state snapshots
/// </summary>
public class Memento<T>
{
    public T State { get; private set; }
    public DateTime Timestamp { get; private set; }

    public Memento(T state)
    {
        State = state;
        Timestamp = DateTime.Now;
    }
}

public class Originator<T>
{
    public T State { get; set; }

    public Originator(T initialState)
    {
        State = initialState;
    }

    public Memento<T> SaveState()
    {
        return new Memento<T>(State);
    }

    public void RestoreState(Memento<T> memento)
    {
        State = memento.State;
    }
}

public class Caretaker<T>
{
    private readonly Stack<Memento<T>> _history = new();
    private readonly Stack<Memento<T>> _future = new();

    public void Save(Memento<T> memento)
    {
        _history.Push(memento);
        _future.Clear();
    }

    public Memento<T>? Undo()
    {
        if (_history.Count == 0)
            return null;

        var memento = _history.Pop();
        _future.Push(memento);
        return memento;
    }

    public Memento<T>? Redo()
    {
        if (_future.Count == 0)
            return null;

        var memento = _future.Pop();
        _history.Push(memento);
        return memento;
    }

    public bool CanUndo => _history.Count > 0;
    public bool CanRedo => _future.Count > 0;
}
