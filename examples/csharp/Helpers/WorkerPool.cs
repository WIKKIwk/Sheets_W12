namespace W12CSheets.Client.Helpers;

/// <summary>
/// Worker pool for parallel task execution
/// </summary>
public class WorkerPool
{
    private readonly int _workerCount;
    private readonly BlockingCollection<Action> _workQueue = new();
    private readonly List<Task> _workers = new();
    private readonly CancellationTokenSource _cts = new();

    public WorkerPool(int workerCount = 4)
    {
        _workerCount = workerCount;
        Start();
    }

    private void Start()
    {
        for (int i = 0; i < _workerCount; i++)
        {
            var worker = Task.Run(async () =>
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var work = _workQueue.Take(_cts.Token);
                        work();
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            });
            _workers.Add(worker);
        }
    }

    public void QueueWork(Action work)
    {
        _workQueue.Add(work);
    }

    public async Task QueueWorkAsync(Func<Task> work)
    {
        var tcs = new TaskCompletionSource<bool>();
        _workQueue.Add(() =>
        {
            try
            {
                work().Wait();
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        await tcs.Task;
    }

    public int PendingWorkCount => _workQueue.Count;

    public async Task StopAsync()
    {
        _cts.Cancel();
        _workQueue.CompleteAdding();
        await Task.WhenAll(_workers);
    }
}
