namespace W12CSheets.Client.Helpers;

/// <summary>
/// Benchmark helper for performance testing
/// </summary>
public static class BenchmarkHelper
{
    /// <summary>
    /// Benchmark action execution
    /// </summary>
    public static BenchmarkResult Benchmark(Action action, int iterations = 1000)
    {
        // Warmup
        action();
        
        var times = new List<long>();
        var sw = new System.Diagnostics.Stopwatch();
        
        for (int i = 0; i < iterations; i++)
        {
            sw.Restart();
            action();
            sw.Stop();
            times.Add(sw.ElapsedMilliseconds);
        }
        
        return new BenchmarkResult
        {
            Iterations = iterations,
            TotalTime = TimeSpan.FromMilliseconds(times.Sum()),
            AverageTime = TimeSpan.FromMilliseconds(times.Average()),
            MinTime = TimeSpan.FromMilliseconds(times.Min()),
            MaxTime = TimeSpan.FromMilliseconds(times.Max())
        };
    }

    /// <summary>
    /// Benchmark async task execution
    /// </summary>
    public static async Task<BenchmarkResult> BenchmarkAsync(Func<Task> taskFactory, int iterations = 1000)
    {
        // Warmup
        await taskFactory();
        
        var times = new List<long>();
        var sw = new System.Diagnostics.Stopwatch();
        
        for (int i = 0; i < iterations; i++)
        {
            sw.Restart();
            await taskFactory();
            sw.Stop();
            times.Add(sw.ElapsedMilliseconds);
        }
        
        return new BenchmarkResult
        {
            Iterations = iterations,
            TotalTime = TimeSpan.FromMilliseconds(times.Sum()),
            AverageTime = TimeSpan.FromMilliseconds(times.Average()),
            MinTime = TimeSpan.FromMilliseconds(times.Min()),
            MaxTime = TimeSpan.FromMilliseconds(times.Max())
        };
    }
}

public class BenchmarkResult
{
    public int Iterations { get; set; }
    public TimeSpan TotalTime { get; set; }
    public TimeSpan AverageTime { get; set; }
    public TimeSpan MinTime { get; set; }
    public TimeSpan MaxTime { get; set; }

    public override string ToString()
    {
        return $"Iterations: {Iterations}, Total: {TotalTime.TotalMilliseconds:F2}ms, " +
               $"Avg: {AverageTime.TotalMilliseconds:F2}ms, " +
               $"Min: {MinTime.TotalMilliseconds:F2}ms, " +
               $"Max: {MaxTime.TotalMilliseconds:F2}ms";
    }
}
