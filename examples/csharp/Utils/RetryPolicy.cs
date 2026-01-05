namespace W12CSheets.Client.Utils;

/// <summary>
/// Retry policy for handling transient failures
/// </summary>
public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly int _delayMs;
    private readonly bool _exponentialBackoff;
    private readonly Logger _logger;

    public RetryPolicy(int maxRetries = 3, int delayMs = 1000, bool exponentialBackoff = true)
    {
        _maxRetries = maxRetries;
        _delayMs = delayMs;
        _exponentialBackoff = exponentialBackoff;
        _logger = new Logger();
    }

    /// <summary>
    /// Execute operation with retry logic
    /// </summary>
    public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, string operationName = "Operation")
    {
        int attempt = 0;
        
        while (true)
        {
            try
            {
                attempt++;
                _logger.Debug($"{operationName}: Attempt {attempt}/{_maxRetries + 1}");
                
                var result = await operation();
                
                if (attempt > 1)
                {
                    _logger.Info($"{operationName}: Succeeded after {attempt} attempts");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                if (attempt > _maxRetries)
                {
                    _logger.Error($"{operationName}: Failed after {attempt} attempts", ex);
                    throw;
                }

                var delay = CalculateDelay(attempt);
                _logger.Warning($"{operationName}: Failed on attempt {attempt}, retrying in {delay}ms");
                
                await Task.Delay(delay);
            }
        }
    }

    /// <summary>
    /// Execute operation with retry logic (synchronous)
    /// </summary>
    public T Execute<T>(Func<T> operation, string operationName = "Operation")
    {
        int attempt = 0;
        
        while (true)
        {
            try
            {
                attempt++;
                _logger.Debug($"{operationName}: Attempt {attempt}/{_maxRetries + 1}");
                
                var result = operation();
                
                if (attempt > 1)
                {
                    _logger.Info($"{operationName}: Succeeded after {attempt} attempts");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                if (attempt > _maxRetries)
                {
                    _logger.Error($"{operationName}: Failed after {attempt} attempts", ex);
                    throw;
                }

                var delay = CalculateDelay(attempt);
                _logger.Warning($"{operationName}: Failed on attempt {attempt}, retrying in {delay}ms");
                
                Thread.Sleep(delay);
            }
        }
    }

    private int CalculateDelay(int attempt)
    {
        if (!_exponentialBackoff)
        {
            return _delayMs;
        }

        // Exponential backoff: delay * 2^(attempt-1)
        return _delayMs * (int)Math.Pow(2, attempt - 1);
    }
}
