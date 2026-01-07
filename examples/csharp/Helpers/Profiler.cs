namespace W12CSheets.Client.Helpers;

/// <summary>
/// Profiler for performance measurement
/// </summary>
public class Profiler
{
    private readonly Dictionary<string, ProfilerEntry> _entries = new();
    private readonly object _lock = new();

    public IDisposable Profile(string name)
    {
        return new ProfilerScope(this, name);
    }

    internal void RecordExecution(string name, TimeSpan duration)
    {
        lock (_lock)
        {
            if (!_entries.ContainsKey(name))
            {
                _entries[name] = new ProfilerEntry { Name = name };
            }

            var entry = _entries[name];
            entry.TotalExecutions++;
            entry.TotalDuration += duration;
            
            if (duration < entry.MinDuration || entry.MinDuration == TimeSpan.Zero)
            {
                entry.MinDuration = duration;
            }
            if (duration > entry.MaxDuration)
            {
                entry.MaxDuration = duration;
            }
        }
    }

    public List<ProfilerEntry> GetResults()
    {
        lock (_lock)
        {
            return _entries.Values.ToList();
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _entries.Clear();
        }
    }

    private class ProfilerScope : IDisposable
    {
        private readonly Profiler _profiler;
        private readonly string _name;
        private readonly System.Diagnostics.Stopwatch _sw;

        public ProfilerScope(Profiler profiler, string name)
        {
            _profiler = profiler;
            _name = name;
            _sw = System.Diagnostics.Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _sw.Stop();
            _profiler.RecordExecution(_name, _sw.Elapsed);
        }
    }

    public class ProfilerEntry
    {
        public string Name { get; set; } = "";
        public int TotalExecutions { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public TimeSpan MinDuration { get; set; }
        public TimeSpan MaxDuration { get; set; }
        public TimeSpan AverageDuration => TotalExecutions > 0 
            ? TimeSpan.FromMilliseconds(TotalDuration.TotalMilliseconds / TotalExecutions)
            : TimeSpan.Zero;
    }
}
