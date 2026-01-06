namespace W12CSheets.Client.Helpers;

/// <summary>
/// Threading helper utilities
/// </summary>
public static class ThreadingHelper
{
    /// <summary>
    /// Run action on background thread
    /// </summary>
    public static Task RunAsync(Action action)
    {
        return Task.Run(action);
    }

    /// <summary>
    /// Run function on background thread
    /// </summary>
    public static Task<T> RunAsync<T>(Func<T> function)
    {
        return Task.Run(function);
    }

    /// <summary>
    /// Run multiple tasks in parallel
    /// </summary>
    public static async Task RunParallel(params Action[] actions)
    {
        var tasks = actions.Select(action => Task.Run(action));
        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Run with maximum degree of parallelism
    /// </summary>
    public static void ParallelForEach<T>(IEnumerable<T> source, Action<T> action, int maxDegreeOfParallelism = 4)
    {
        Parallel.ForEach(source, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, action);
    }

    /// <summary>
    /// Sleep for specified duration
    /// </summary>
    public static void Sleep(TimeSpan duration)
    {
        Thread.Sleep(duration);
    }

    /// <summary>
    /// Get current thread ID
    /// </summary>
    public static int GetCurrentThreadId()
    {
        return Environment.CurrentManagedThreadId;
    }

    /// <summary>
    /// Create cancellation token source with timeout
    /// </summary>
    public static CancellationTokenSource CreateCancellationTokenSource(TimeSpan timeout)
    {
        return new CancellationTokenSource(timeout);
    }

    /// <summary>
    /// Execute with timeout
    /// </summary>
    public static async Task<T> WithTimeout<T>(Task<T> task, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);
        var completedTask = await Task.WhenAny(task, Task.Delay(timeout, cts.Token));
        
        if (completedTask != task)
        {
            throw new TimeoutException("Operation timed out");
        }
        
        cts.Cancel();
        return await task;
    }
}
