namespace W12CSheets.Client.Helpers;

/// <summary>
/// Retry policy for handling transient failures
/// </summary>
public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _delay;
    private readonly double _backoffMultiplier;

    public RetryPolicy(int maxRetries = 3, TimeSpan? delay = null, double backoffMultiplier = 2.0)
    {
        _maxRetries = maxRetries;
        _delay = delay ?? TimeSpan.FromSeconds(1);
        _backoffMultiplier = backoffMultiplier;
    }

    /// <summary>
    /// Execute action with retry
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        var attempt = 0;
        var currentDelay = _delay;

        while (true)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                attempt++;
                if (attempt >= _maxRetries)
                {
                    throw;
                }

                await Task.Delay(currentDelay);
                currentDelay = TimeSpan.FromMilliseconds(currentDelay.TotalMilliseconds * _backoffMultiplier);
            }
        }
    }

    /// <summary>
    /// Execute action with retry (no return value)
    /// </summary>
    public async Task ExecuteAsync(Func<Task> action)
    {
        await ExecuteAsync(async () =>
        {
            await action();
            return true;
        });
    }

    /// <summary>
    /// Execute with retry and specific exception handling
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, Func<Exception, bool> shouldRetry)
    {
        var attempt = 0;
        var currentDelay = _delay;

        while (true)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                if (!shouldRetry(ex) || attempt >= _maxRetries)
                {
                    throw;
                }

                attempt++;
                await Task.Delay(currentDelay);
                currentDelay = TimeSpan.FromMilliseconds(currentDelay.TotalMilliseconds * _backoffMultiplier);
            }
        }
    }
}
