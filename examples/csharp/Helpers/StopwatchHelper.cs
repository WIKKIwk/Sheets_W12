namespace W12CSheets.Client.Helpers;

/// <summary>
/// Stopwatch wrapper for timing operations
/// </summary>
public class StopwatchHelper
{
    private readonly System.Diagnostics.Stopwatch _stopwatch = new();
    private readonly List<(string Name, TimeSpan Duration)> _laps = new();

    /// <summary>
    /// Start stopwatch
    /// </summary>
    public void Start()
    {
        _stopwatch.Start();
    }

    /// <summary>
    /// Stop stopwatch
    /// </summary>
    public TimeSpan Stop()
    {
        _stopwatch.Stop();
        return _stopwatch.Elapsed;
    }

    /// <summary>
    /// Reset stop watch
    /// </summary>
    public void Reset()
    {
        _stopwatch.Reset();
        _laps.Clear();
    }

    /// <summary>
    /// Record lap time
    /// </summary>
    public TimeSpan Lap(string? name = null)
    {
        var lapTime = _stopwatch.Elapsed;
        _laps.Add((name ?? $"Lap {_laps.Count + 1}", lapTime));
        return lapTime;
    }

    /// <summary>
    /// Get all lap times
    /// </summary>
    public IReadOnlyList<(string Name, TimeSpan Duration)> GetLaps()
    {
        return _laps.AsReadOnly();
    }

    /// <summary>
    /// Get elapsed time
    /// </summary>
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    /// <summary>
    /// Check if running
    /// </summary>
    public bool IsRunning => _stopwatch.IsRunning;
}
