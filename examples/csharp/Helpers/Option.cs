namespace W12CSheets.Client.Helpers;

/// <summary>
/// Option type for handling nullable values
/// </summary>
public class Option<T>
{
    public bool HasValue { get; private set; }
    private readonly T? _value;

    private Option(bool hasValue, T? value)
    {
        HasValue = hasValue;
        _value = value;
    }

    /// <summary>
    /// Create some option with value
    /// </summary>
    public static Option<T> Some(T value)
    {
        return new Option<T>(true, value);
    }

    /// <summary>
    /// Create none option
    /// </summary>
    public static Option<T> None()
    {
        return new Option<T>(false, default);
    }

    /// <summary>
    /// Get value or default
    /// </summary>
    public T GetValueOrDefault(T defaultValue)
    {
        return HasValue ? _value! : defaultValue;
    }

    /// <summary>
    /// Get value or throw
    /// </summary>
    public T GetValueOrThrow()
    {
        if (!HasValue)
        {
            throw new InvalidOperationException("Option has no value");
        }
        return _value!;
    }

    /// <summary>
    /// Match option with handlers
    /// </summary>
    public TResult Match<TResult>(Func<T, TResult> onSome, Func<TResult> onNone)
    {
        return HasValue ? onSome(_value!) : onNone();
    }

    /// <summary>
    /// Map value if some
    /// </summary>
    public Option<TOut> Map<TOut>(Func<T, TOut> mapper)
    {
        return HasValue 
            ? Option<TOut>.Some(mapper(_value!))
            : Option<TOut>.None();
    }

    /// <summary>
    /// Bind operation for chaining
    /// </summary>
    public Option<TOut> Bind<TOut>(Func<T, Option<TOut>> binder)
    {
        return HasValue 
            ? binder(_value!)
            : Option<TOut>.None();
    }
}
