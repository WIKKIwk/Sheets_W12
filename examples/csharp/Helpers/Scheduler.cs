namespace W12CSheets.Client.Helpers;

/// <summary>
/// Scheduler for delayed and recurring tasks
/// </summary>
public class Scheduler
{
    private readonly List<ScheduledTask> _tasks = new();
    private readonly object _lock = new();

    /// <summary>
    /// Schedule one-time task
    /// </summary>
    public Guid Schedule(Action action, TimeSpan delay, string? name = null)
    {
        var id = Guid.NewGuid();
        var task = new ScheduledTask
        {
            Id = id,
            Name = name ?? id.ToString(),
            Action = action,
            Delay = delay,
            IsRecurring = false
        };

        lock (_lock)
        {
            _tasks.Add(task);
        }

        Task.Delay(delay).ContinueWith(_ =>
        {
            action();
            lock (_lock)
            {
                _tasks.RemoveAll(t => t.Id == id);
            }
        });

        return id;
    }

    /// <summary>
    /// Schedule recurring task
    /// </summary>
    public Guid ScheduleRecurring(Action action, TimeSpan interval, string? name = null)
    {
        var id = Guid.NewGuid();
        var task = new ScheduledTask
        {
            Id = id,
            Name = name ?? id.ToString(),
            Action = action,
            Delay = interval,
            IsRecurring = true
        };

        lock (_lock)
        {
            _tasks.Add(task);
        }

        var timer = new System.Threading.Timer(_ => action(), null, TimeSpan.Zero, interval);
        task.Timer = timer;

        return id;
    }

    /// <summary>
    /// Cancel scheduled task
    /// </summary>
    public bool Cancel(Guid id)
    {
        lock (_lock)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.Timer?.Dispose();
                _tasks.Remove(task);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Get all scheduled tasks
    /// </summary>
    public List<string> GetScheduledTasks()
    {
        lock (_lock)
        {
            return _tasks.Select(t => t.Name).ToList();
        }
    }

    private class ScheduledTask
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public Action Action { get; set; } = null!;
        public TimeSpan Delay { get; set; }
        public bool IsRecurring { get; set; }
        public System.Threading.Timer? Timer { get; set; }
    }
}
