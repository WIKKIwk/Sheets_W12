namespace W12CSheets.Client.Patterns;

/// <summary>
/// Visitor pattern for operations on object structures
/// </summary>
public interface IVisitor<T>
{
    void Visit(T element);
}

public interface IVisitable<T>
{
    void Accept(IVisitor<T> visitor);
}

public abstract class Element<T> : IVisitable<T>
{
    public abstract void Accept(IVisitor<T> visitor);
}

public class ConcreteElementA<T> : Element<T>
{
    public T Data { get; set; }

    public ConcreteElementA(T data)
    {
        Data = data;
    }

    public override void Accept(IVisitor<T> visitor)
    {
        visitor.Visit(Data);
    }
}

public class ConcreteElementB<T> : Element<T>
{
    public T Data { get; set; }

    public ConcreteElementB(T data)
    {
        Data = data;
    }

    public override void Accept(IVisitor<T> visitor)
    {
        visitor.Visit(Data);
    }
}

public class ObjectStructure<T>
{
    private readonly List<Element<T>> _elements = new();

    public void Attach(Element<T> element)
    {
        _elements.Add(element);
    }

    public void Detach(Element<T> element)
    {
        _elements.Remove(element);
    }

    public void Accept(IVisitor<T> visitor)
    {
        foreach (var element in _elements)
        {
            element.Accept(visitor);
        }
    }
}
