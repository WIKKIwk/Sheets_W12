namespace W12CSheets.Client.Helpers;

/// <summary>
/// Middleware pipeline for request processing
/// </summary>
public class MiddlewarePipeline<TContext>
{
    private readonly List<Func<TContext, Func<Task>, Task>> _middlewares = new();

    /// <summary>
    /// Add middleware to pipeline
    /// </summary>
    public MiddlewarePipeline<TContext> Use(Func<TContext, Func<Task>, Task> middleware)
    {
        _middlewares.Add(middleware);
        return this;
    }

    /// <summary>
    /// Execute pipeline
    /// </summary>
    public async Task ExecuteAsync(TContext context)
    {
        var index = 0;

        async Task Next()
        {
            if (index < _middlewares.Count)
            {
                var current = _middlewares[index];
                index++;
                await current(context, Next);
            }
        }

        await Next();
    }

    /// <summary>
    /// Clear all middlewares
    /// </summary>
    public void Clear()
    {
        _middlewares.Clear();
    }

    /// <summary>
    /// Get middleware count
    /// </summary>
    public int Count => _middlewares.Count;
}
