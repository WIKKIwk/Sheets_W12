namespace W12CSheets.Client.Helpers;

/// <summary>
/// Result type for operation outcomes
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? Error { get; private set; }

    private Result(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    /// <summary>
    /// Create success result
    /// </summary>
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    /// <summary>
    /// Create failure result
    /// </summary>
    public static Result<T> Failure(string error)
    {
        return new Result<T>(false, default, error);
    }

    /// <summary>
    /// Match result with success and failure handlers
    /// </summary>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }

    /// <summary>
    /// Map value if success
    /// </summary>
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
    {
        return IsSuccess 
            ? Result<TOut>.Success(mapper(Value!))
            : Result<TOut>.Failure(Error!);
    }

    /// <summary>
    /// Bind operation for chaining
    /// </summary>
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)
    {
        return IsSuccess 
            ? binder(Value!)
            : Result<TOut>.Failure(Error!);
    }
}
