using System.Diagnostics;

namespace W12CSheets.Client.Utils;

/// <summary>
/// Performance measurement utility
/// </summary>
public class PerformanceMonitor
{
    private readonly Dictionary<string, OperationMetrics> _metrics = new();
    private readonly object _lock = new();

    /// <summary>
    /// Measure operation performance
    /// </summary>
    public async Task<T> MeasureAsync<T>(string operationName, Func<Task<T>> operation)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await operation();
            sw.Stop();
            RecordSuccess(operationName, sw.ElapsedMilliseconds);
            return result;
        }
        catch
        {
            sw.Stop();
            RecordFailure(operationName, sw.ElapsedMilliseconds);
            throw;
        }
    }

    /// <summary>
    /// Measure synchronous operation
    /// </summary>
    public T Measure<T>(string operationName, Func<T> operation)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = operation();
            sw.Stop();
            RecordSuccess(operationName, sw.ElapsedMilliseconds);
            return result;
        }
        catch
        {
            sw.Stop();
            RecordFailure(operationName, sw.ElapsedMilliseconds);
            throw;
        }
    }

    /// <summary>
    /// Get metrics for specific operation
    /// </summary>
    public OperationMetrics? GetMetrics(string operationName)
    {
        lock (_lock)
        {
            return _metrics.GetValueOrDefault(operationName);
        }
    }

    /// <summary>
    /// Get all metrics
    /// </summary>
    public Dictionary<string, OperationMetrics> GetAllMetrics()
    {
        lock (_lock)
        {
            return new Dictionary<string, OperationMetrics>(_metrics);
        }
    }

    /// <summary>
    /// Clear all metrics
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _metrics.Clear();
        }
    }

    /// <summary>
    /// Print performance report
    /// </summary>
    public void PrintReport()
    {
        lock (_lock)
        {
            Console.WriteLine("\n=== Performance Report ===\n");
            foreach (var kvp in _metrics.OrderBy(x => x.Key))
            {
                Console.WriteLine($"{kvp.Key}:");
                Console.WriteLine($"  Total Calls: {kvp.Value.TotalCalls}");
                Console.WriteLine($"  Successful: {kvp.Value.SuccessfulCalls}");
                Console.WriteLine($"  Failed: {kvp.Value.FailedCalls}");
                Console.WriteLine($"  Avg Duration: {kvp.Value.AverageDuration:F2}ms");
                Console.WriteLine($"  Min Duration: {kvp.Value.MinDuration}ms");
                Console.WriteLine($"  Max Duration: {kvp.Value.MaxDuration}ms");
                Console.WriteLine();
            }
        }
    }

    private void RecordSuccess(string operationName, long durationMs)
    {
        lock (_lock)
        {
            if (!_metrics.ContainsKey(operationName))
            {
                _metrics[operationName] = new OperationMetrics { Name = operationName };
            }

            var metrics = _metrics[operationName];
            metrics.TotalCalls++;
            metrics.SuccessfulCalls++;
            metrics.TotalDuration += durationMs;
            metrics.MinDuration = Math.Min(metrics.MinDuration ?? long.MaxValue, durationMs);
            metrics.MaxDuration = Math.Max(metrics.MaxDuration ?? 0, durationMs);
        }
    }

    private void RecordFailure(string operationName, long durationMs)
    {
        lock (_lock)
        {
            if (!_metrics.ContainsKey(operationName))
            {
                _metrics[operationName] = new OperationMetrics { Name = operationName };
            }

            var metrics = _metrics[operationName];
            metrics.TotalCalls++;
            metrics.FailedCalls++;
            metrics.TotalDuration += durationMs;
        }
    }
}

public class OperationMetrics
{
    public string Name { get; set; } = "";
    public int TotalCalls { get; set; }
    public int SuccessfulCalls { get; set; }
    public int FailedCalls { get; set; }
    public long TotalDuration { get; set; }
    public long? MinDuration { get; set; }
    public long? MaxDuration { get; set; }

    public double AverageDuration => TotalCalls > 0 ? (double)TotalDuration / TotalCalls : 0;
    public double SuccessRate => TotalCalls > 0 ? (double)SuccessfulCalls / TotalCalls * 100 : 0;
}
