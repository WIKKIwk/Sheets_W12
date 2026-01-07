namespace W12CSheets.Client.Helpers;

/// <summary>
/// Batch processor for processing items in batches
/// </summary>
public class BatchProcessor<T>
{
    private readonly int _batchSize;
    private readonly Func<List<T>, Task> _processor;

    public BatchProcessor(int batchSize, Func<List<T>, Task> processor)
    {
        _batchSize = batchSize;
        _processor = processor;
    }

    /// <summary>
    /// Process items in batches
    /// </summary>
    public async Task ProcessAsync(IEnumerable<T> items)
    {
        var batch = new List<T>();

        foreach (var item in items)
        {
            batch.Add(item);

            if (batch.Count >= _batchSize)
            {
                await _processor(batch);
                batch.Clear();
            }
        }

        if (batch.Count > 0)
        {
            await _processor(batch);
        }
    }

    /// <summary>
    /// Process items in batches with concurrency
    /// </summary>
    public async Task ProcessConcurrentAsync(IEnumerable<T> items, int maxConcurrency)
    {
        var batches = items
            .Select((item, index) => new { item, index })
            .GroupBy(x => x.index / _batchSize)
            .Select(g => g.Select(x => x.item).ToList());

        var semaphore = new SemaphoreSlim(maxConcurrency);
        var tasks = batches.Select(async batch =>
        {
            await semaphore.WaitAsync();
            try
            {
                await _processor(batch);
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(tasks);
    }
}
