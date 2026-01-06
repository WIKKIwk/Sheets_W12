namespace W12CSheets.Client.Patterns;

/// <summary>
/// Chain of Responsibility pattern
/// </summary>
public interface IHandler<T>
{
    IHandler<T>? NextHandler { get; set; }
    void Handle(T request);
}

public abstract class Handler<T> : IHandler<T>
{
    public IHandler<T>? NextHandler { get; set; }

    public virtual void Handle(T request)
    {
        if (CanHandle(request))
        {
            ProcessRequest(request);
        }
        else if (NextHandler != null)
        {
            NextHandler.Handle(request);
        }
    }

    protected abstract bool CanHandle(T request);
    protected abstract void ProcessRequest(T request);

    public IHandler<T> SetNext(IHandler<T> handler)
    {
        NextHandler = handler;
        return handler;
    }
}

/// <summary>
/// Example handler
/// </summary>
public class ConcreteHandlerA<T> : Handler<T>
{
    private readonly Func<T, bool> _canHandlePredicate;
    private readonly Action<T> _processAction;

    public ConcreteHandlerA(Func<T, bool> canHandlePredicate, Action<T> processAction)
    {
        _canHandlePredicate = canHandlePredicate;
        _processAction = processAction;
    }

    protected override bool CanHandle(T request)
    {
        return _canHandlePredicate(request);
    }

    protected override void ProcessRequest(T request)
    {
        _processAction(request);
    }
}
