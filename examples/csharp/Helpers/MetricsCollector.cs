namespace W12CSheets.Client.Helpers;

/// <summary>
/// Metrics collector for application monitoring
/// </summary>
public class MetricsCollector
{
    private readonly Dictionary<string, long> _counters = new();
    private readonly Dictionary<string, List<double>> _gauges = new();
    private readonly object _lock = new();

    /// <summary>
    /// Increment counter
    /// </summary>
    public void IncrementCounter(string name, long value = 1)
    {
        lock (_lock)
        {
            if (!_counters.ContainsKey(name))
            {
                _counters[name] = 0;
            }
            _counters[name] += value;
        }
    }

    /// <summary>
    /// Record gauge value
    /// </summary>
    public void RecordGauge(string name, double value)
    {
        lock (_lock)
        {
            if (!_gauges.ContainsKey(name))
            {
                _gauges[name] = new List<double>();
            }
            _gauges[name].Add(value);
        }
    }

    /// <summary>
    /// Get counter value
    /// </summary>
    public long GetCounter(string name)
    {
        lock (_lock)
        {
            return _counters.GetValueOrDefault(name, 0);
        }
    }

    /// <summary>
    /// Get gauge statistics
    /// </summary>
    public GaugeStatistics? GetGaugeStatistics(string name)
    {
        lock (_lock)
        {
            if (!_gauges.ContainsKey(name) || _gauges[name].Count == 0)
            {
                return null;
            }

            var values = _gauges[name];
            return new GaugeStatistics
            {
                Count = values.Count,
                Min = values.Min(),
                Max = values.Max(),
                Average = values.Average(),
                Latest = values.Last()
            };
        }
    }

    /// <summary>
    /// Get all metrics
    /// </summary>
    public MetricsSnapshot GetSnapshot()
    {
        lock (_lock)
        {
            return new MetricsSnapshot
            {
                Counters = new Dictionary<string, long>(_counters),
                Gauges = _gauges.Select(kvp => new
                {
                    kvp.Key,
                    Stats = GetGaugeStatistics(kvp.Key)
                }).ToDictionary(x => x.Key, x => x.Stats!)
            };
        }
    }

    /// <summary>
    /// Reset all metrics
    /// </summary>
    public void Reset()
    {
        lock (_lock)
        {
            _counters.Clear();
            _gauges.Clear();
        }
    }
}

public class GaugeStatistics
{
    public int Count { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public double Average { get; set; }
    public double Latest { get; set; }
}

public class MetricsSnapshot
{
    public Dictionary<string, long> Counters { get; set; } = new();
    public Dictionary<string, GaugeStatistics> Gauges { get; set; } = new();
}
