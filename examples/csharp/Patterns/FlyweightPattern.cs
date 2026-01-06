namespace W12CSheets.Client.Patterns;

/// <summary>
/// Flyweight pattern for memory optimization
/// </summary>
public class Flyweight<T>
{
    private readonly T _sharedState;

    public Flyweight(T sharedState)
    {
        _sharedState = sharedState;
    }

    public void Operation(T uniqueState)
    {
        Console.WriteLine($"Flyweight: Displaying shared ({_sharedState}) and unique ({uniqueState}) state.");
    }
}

public class FlyweightFactory<T>
{
    private readonly Dictionary<string, Flyweight<T>> _flyweights = new();

    public Flyweight<T> GetFlyweight(string key, T sharedState)
    {
        if (!_flyweights.ContainsKey(key))
        {
            _flyweights[key] = new Flyweight<T>(sharedState);
        }

        return _flyweights[key];
    }

    public int GetFlyweightCount() => _flyweights.Count;

    public void ListFlyweights()
    {
        Console.WriteLine($"FlyweightFactory: I have {_flyweights.Count} flyweights:");
        foreach (var key in _flyweights.Keys)
        {
            Console.WriteLine($"  - {key}");
        }
    }
}
