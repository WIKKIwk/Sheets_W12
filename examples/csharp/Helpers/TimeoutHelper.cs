namespace W12CSheets.Client.Helpers;

/// <summary>
/// Timeout helper for enforcing operation timeouts
/// </summary>
public static class TimeoutHelper
{
    /// <summary>
    /// Execute with timeout
    /// </summary>
    public static async Task<T> WithTimeout<T>(Func<Task<T>> operation, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource();
        var task = operation();
        var timeoutTask = Task.Delay(timeout, cts.Token);

        var completedTask = await Task.WhenAny(task, timeoutTask);

        if (completedTask == timeoutTask)
        {
            throw new TimeoutException($"Operation timed out after {timeout.TotalSeconds} seconds");
        }

        cts.Cancel();
        return await task;
    }

    /// <summary>
    /// Execute with timeout (no return value)
    /// </summary>
    public static async Task WithTimeout(Func<Task> operation, TimeSpan timeout)
    {
        await WithTimeout(async () =>
        {
            await operation();
            return true;
        }, timeout);
    }

    /// <summary>
    /// Execute with timeout and cancellation token
    /// </summary>
    public static async Task<T> WithTimeout<T>(
        Func<CancellationToken, Task<T>> operation,
        TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        return await operation(cts.Token);
    }
}
