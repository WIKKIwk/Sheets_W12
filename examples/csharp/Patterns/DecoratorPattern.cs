namespace W12CSheets.Client.Patterns;

/// <summary>
/// Decorator pattern helper
/// </summary>
public interface IComponent
{
    string Operation();
}

public class ConcreteComponent : IComponent
{
    public string Operation()
    {
        return "ConcreteComponent";
    }
}

public abstract class Decorator : IComponent
{
    protected IComponent _component;

    public Decorator(IComponent component)
    {
        _component = component;
    }

    public virtual string Operation()
    {
        return _component.Operation();
    }
}

public class ConcreteDecoratorA : Decorator
{
    public ConcreteDecoratorA(IComponent component) : base(component) { }

    public override string Operation()
    {
        return $"DecoratorA({base.Operation()})";
    }
}

public class ConcreteDecoratorB : Decorator
{
    public ConcreteDecoratorB(IComponent component) : base(component) { }

    public override string Operation()
    {
        return $"DecoratorB({base.Operation()})";
    }
}
