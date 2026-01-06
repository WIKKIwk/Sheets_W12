namespace W12CSheets.Client.Helpers;

/// <summary>
/// Histogram for tracking value distributions
/// </summary>
public class Histogram
{
    private readonly Dictionary<double, int> _buckets = new();
    private readonly List<double> _values = new();
    private readonly object _lock = new();

    /// <summary>
    /// Record a value
    /// </summary>
    public void Record(double value)
    {
        lock (_lock)
        {
            _values.Add(value);
            
            var bucket = Math.Floor(value / 10) * 10;
            if (!_buckets.ContainsKey(bucket))
            {
                _buckets[bucket] = 0;
            }
            _buckets[bucket]++;
        }
    }

    /// <summary>
    /// Get percentile
    /// </summary>
    public double GetPercentile(double percentile)
    {
        lock (_lock)
        {
            if (_values.Count == 0)
                return 0;

            var sorted = _values.OrderBy(x => x).ToList();
            int index = (int)Math.Ceiling(percentile / 100.0 * sorted.Count) - 1;
            return sorted[Math.Max(0, index)];
        }
    }

    /// <summary>
    /// Get statistics
    /// </summary>
    public HistogramStatistics GetStatistics()
    {
        lock (_lock)
        {
            if (_values.Count == 0)
            {
                return new HistogramStatistics();
            }

            return new HistogramStatistics
            {
                Count = _values.Count,
                Min = _values.Min(),
                Max = _values.Max(),
                Mean = _values.Average(),
                P50 = GetPercentile(50),
                P90 = GetPercentile(90),
                P95 = GetPercentile(95),
                P99 = GetPercentile(99)
            };
        }
    }

    /// <summary>
    /// Clear histogram
    /// </summary>
    public void Clear()
    {
        lock (_lock)
        {
            _buckets.Clear();
            _values.Clear();
        }
    }
}

public class HistogramStatistics
{
    public int Count { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public double Mean { get; set; }
    public double P50 { get; set; }
    public double P90 { get; set; }
    public double P95 { get; set; }
    public double P99 { get; set; }
}
