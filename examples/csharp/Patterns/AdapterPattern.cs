namespace W12CSheets.Client.Patterns;

/// <summary>
/// Adapter pattern for interface conversion
/// </summary>
public interface ITarget
{
    string Request();
}

public class Adaptee
{
    public string SpecificRequest()
    {
        return "Adaptee specific request";
    }
}

public class Adapter : ITarget
{
    private readonly Adaptee _adaptee;

    public Adapter(Adaptee adaptee)
    {
        _adaptee = adaptee;
    }

    public string Request()
    {
        return $"Adapter: (TRANSLATED) {_adaptee.SpecificRequest()}";
    }
}

/// <summary>
/// Generic adapter
/// </summary>
public class GenericAdapter<TSource, TTarget> where TTarget : class, new()
{
    private readonly Func<TSource, TTarget> _adaptFunction;

    public GenericAdapter(Func<TSource, TTarget> adaptFunction)
    {
        _adaptFunction = adaptFunction;
    }

    public TTarget Adapt(TSource source)
    {
        return _adaptFunction(source);
    }

    public IEnumerable<TTarget> AdaptMany(IEnumerable<TSource> sources)
    {
        return sources.Select(s => _adaptFunction(s));
    }
}
