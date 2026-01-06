namespace W12CSheets.Client.Patterns;

/// <summary>
/// Proxy pattern for controlled access
/// </summary>
public interface ISubject
{
    string Request();
}

public class RealSubject : ISubject
{
    public string Request()
    {
        return "RealSubject: Handling request";
    }
}

public class Proxy : ISubject
{
    private RealSubject? _realSubject;
    private readonly Func<bool> _accessCheck;

    public Proxy(Func<bool> accessCheck)
    {
        _accessCheck = accessCheck;
    }

    public string Request()
    {
        if (!_accessCheck())
        {
            return "Proxy: Access denied";
        }

        if (_realSubject == null)
        {
            _realSubject = new RealSubject();
        }

        return _realSubject.Request();
    }
}

/// <summary>
/// Caching proxy
/// </summary>
public class CachingProxy<T> : ISubject where T : class
{
    private readonly ISubject _realSubject;
    private string? _cache;

    public CachingProxy(ISubject realSubject)
    {
        _realSubject = realSubject;
    }

    public string Request()
    {
        if (_cache == null)
        {
            _cache = _realSubject.Request();
        }
        return _cache;
    }

    public void ClearCache()
    {
        _cache = null;
    }
}
