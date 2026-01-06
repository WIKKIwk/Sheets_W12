namespace W12CSheets.Client.Patterns;

/// <summary>
/// Bridge pattern for separating abstraction from implementation
/// </summary>
public interface IImplementation
{
    string OperationImplementation();
}

public abstract class Abstraction
{
    protected IImplementation _implementation;

    protected Abstraction(IImplementation implementation)
    {
        _implementation = implementation;
    }

    public virtual string Operation()
    {
        return $"Abstraction: Base operation with:\n{_implementation.OperationImplementation()}";
    }
}

public class RefinedAbstraction : Abstraction
{
    public RefinedAbstraction(IImplementation implementation) : base(implementation)
    {
    }

    public override string Operation()
    {
        return $"RefinedAbstraction: Extended operation with:\n{_implementation.OperationImplementation()}";
    }
}

public class ConcreteImplementationA : IImplementation
{
    public string OperationImplementation()
    {
        return "ConcreteImplementationA: Platform A implementation";
    }
}

public class ConcreteImplementationB : IImplementation
{
    public string OperationImplementation()
    {
        return "ConcreteImplementationB: Platform B implementation";
    }
}
