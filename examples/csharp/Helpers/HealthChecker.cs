namespace W12CSheets.Client.Helpers;

/// <summary>
/// Health check helper for service monitoring
/// </summary>
public class HealthChecker
{
    private readonly Dictionary<string, HealthCheck> _checks = new();
    private readonly object _lock = new();

    /// <summary>
    /// Register health check
    /// </summary>
    public void RegisterCheck(string name, Func<Task<bool>> checkFunc)
    {
        lock (_lock)
        {
            _checks[name] = new HealthCheck { Name = name, CheckFunc = checkFunc };
        }
    }

    /// <summary>
    /// Execute all health checks
    /// </summary>
    public async Task<HealthReport> CheckHealthAsync()
    {
        var results = new Dictionary<string, HealthCheckResult>();
        
        Dictionary<string, HealthCheck> checksCopy;
        lock (_lock)
        {
            checksCopy = new Dictionary<string, HealthCheck>(_checks);
        }

        foreach (var check in checksCopy.Values)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var isHealthy = await check.CheckFunc();
                sw.Stop();
                
                results[check.Name] = new HealthCheckResult
                {
                    IsHealthy = isHealthy,
                    Duration = sw.Elapsed,
                    Message = isHealthy ? "OK" : "Failed"
                };
            }
            catch (Exception ex)
            {
                sw.Stop();
                results[check.Name] = new HealthCheckResult
                {
                    IsHealthy = false,
                    Duration = sw.Elapsed,
                    Message = ex.Message
                };
            }
        }

        var overallHealthy = results.Values.All(r => r.IsHealthy);
        
        return new HealthReport
        {
            IsHealthy = overallHealthy,
            Checks = results,
            Timestamp = DateTime.Now
        };
    }

    /// <summary>
    /// Remove health check
    /// </summary>
    public void Unregister(string name)
    {
        lock (_lock)
        {
            _checks.Remove(name);
        }
    }

    /// <summary>
    /// Clear all checks
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _checks.Clear();
        }
    }
}

public class HealthCheck
{
    public string Name { get; set; } = "";
    public Func<Task<bool>> CheckFunc { get; set; } = null!;
}

public class HealthCheckResult
{
    public bool IsHealthy { get; set; }
    public TimeSpan Duration { get; set; }
    public string Message { get; set; } = "";
}

public class HealthReport
{
    public bool IsHealthy { get; set; }
    public Dictionary<string, HealthCheckResult> Checks { get; set; } = new();
    public DateTime Timestamp { get; set; }
}
