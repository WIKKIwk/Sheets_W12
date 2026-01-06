namespace W12CSheets.Client.Helpers;

/// <summary>
/// Builder pattern helper for creating objects fluently
/// </summary>
public class BuilderHelper<T> where T : new()
{
    private readonly T _instance;

    public BuilderHelper()
    {
        _instance = new T();
    }

    /// <summary>
    /// Set property value
    /// </summary>
    public BuilderHelper<T> Set(string propertyName, object? value)
    {
        var property = typeof(T).GetProperty(propertyName);
        property?.SetValue(_instance, value);
        return this;
    }

    /// <summary>
    /// Apply custom action
    /// </summary>
    public BuilderHelper<T> With(Action<T> action)
    {
        action(_instance);
        return this;
    }

    /// <summary>
    /// Build the final object
    /// </summary>
    public T Build()
    {
        return _instance;
    }

    /// <summary>
    /// Create new builder instance
    /// </summary>
    public static BuilderHelper<T> Create()
    {
        return new BuilderHelper<T>();
    }
}
