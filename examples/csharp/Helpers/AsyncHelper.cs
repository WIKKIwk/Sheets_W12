namespace W12CSheets.Client.Helpers;

/// <summary>
/// Async helper utilities
/// </summary>
public static class AsyncHelper
{
    /// <summary>
    /// Run async method synchronously
    /// </summary>
    public static T RunSync<T>(Func<Task<T>> task)
    {
        return Task.Run(task).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Run async action synchronously
    /// </summary>
    public static void RunSync(Func<Task> task)
    {
        Task.Run(task).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Run tasks in parallel with max concurrency
    /// </summary>
    public static async Task ForEachAsync<T>(
        IEnumerable<T> source,
        int maxDegreeOfParallelism,
        Func<T, Task> action)
    {
        var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
        var tasks = source.Select(async item =>
        {
            await semaphore.WaitAsync();
            try
            {
                await action(item);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// Fire and forget task
    /// </summary>
    public static void FireAndForget(Task task, Action<Exception>? errorHandler = null)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted && t.Exception != null)
            {
                errorHandler?.Invoke(t.Exception);
            }
        }, TaskContinuationOptions.OnlyOnFaulted);
    }

    /// <summary>
    /// Wait for any task to complete
    /// </summary>
    public static async Task<T> WhenAny<T>(params Task<T>[] tasks)
    {
        var completedTask = await Task.WhenAny(tasks);
        return await completedTask;
    }

    /// <summary>
    /// Wait for all tasks with timeout
    /// </summary>
    public static async Task<bool> WhenAllWithTimeout(TimeSpan timeout, params Task[] tasks)
    {
        var timeoutTask = Task.Delay(timeout);
        var completedTask = await Task.WhenAny(Task.WhenAll(tasks), timeoutTask);
        return completedTask != timeoutTask;
    }
}
