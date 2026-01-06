namespace W12CSheets.Client.Extensions;

/// <summary>
/// Extension methods for Task
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Add timeout to task
    /// </summary>
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
    {
        var timeoutTask = Task.Delay(timeout);
        var completedTask = await Task.WhenAny(task, timeoutTask);
        
        if (completedTask == timeoutTask)
        {
            throw new TimeoutException($"Task timed out after {timeout.TotalSeconds} seconds");
        }
        
        return await task;
    }

    /// <summary>
    /// Execute task and ignore exceptions
    /// </summary>
    public static async Task IgnoreExceptions(this Task task)
    {
        try
        {
            await task;
        }
        catch
        {
            // Ignore
        }
    }

    /// <summary>
    /// Execute task with retry
    /// </summary>
    public static async Task<T> WithRetry<T>(this Func<Task<T>> taskFactory, int maxRetries = 3, TimeSpan? delay = null)
    {
        var retryDelay = delay ?? TimeSpan.FromSeconds(1);
        
        for (int i = 0; i <= maxRetries; i++)
        {
            try
            {
                return await taskFactory();
            }
            catch when (i < maxRetries)
            {
                await Task.Delay(retryDelay);
            }
        }
        
        throw new InvalidOperationException("All retries failed");
    }

    /// <summary>
    /// Execute with cancellation token
    /// </summary>
    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<T>();
        
        using (cancellationToken.Register(() => tcs.TrySetCanceled()))
        {
            var completedTask = await Task.WhenAny(task, tcs.Task);
            return await completedTask;
        }
    }
}
