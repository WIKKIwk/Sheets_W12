namespace W12CSheets.Client.Patterns;

/// <summary>
/// Composite pattern for tree structures
/// </summary>
public interface IComponent<T>
{
    string GetName();
    void Add(IComponent<T> component);
    void Remove(IComponent<T> component);
    IEnumerable<IComponent<T>> GetChildren();
    void Process();
}

public class Leaf<T> : IComponent<T>
{
    private readonly string _name;
    private readonly Action _action;

    public Leaf(string name, Action action)
    {
        _name = name;
        _action = action;
    }

    public string GetName() => _name;

    public void Process()
    {
        _action();
    }

    public void Add(IComponent<T> component)
    {
        throw new NotSupportedException("Cannot add to a leaf");
    }

    public void Remove(IComponent<T> component)
    {
        throw new NotSupportedException("Cannot remove from a leaf");
    }

    public IEnumerable<IComponent<T>> GetChildren()
    {
        return Enumerable.Empty<IComponent<T>>();
    }
}

public class Composite<T> : IComponent<T>
{
    private readonly string _name;
    private readonly List<IComponent<T>> _children = new();

    public Composite(string name)
    {
        _name = name;
    }

    public string GetName() => _name;

    public void Add(IComponent<T> component)
    {
        _children.Add(component);
    }

    public void Remove(IComponent<T> component)
    {
        _children.Remove(component);
    }

    public IEnumerable<IComponent<T>> GetChildren()
    {
        return _children;
    }

    public void Process()
    {
        foreach (var child in _children)
        {
            child.Process();
        }
    }
}
