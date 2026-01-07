namespace W12CSheets.Client.Helpers;

/// <summary>
/// Semaphore pool for resource limiting
/// </summary>
public class SemaphorePool
{
    private readonly SemaphoreSlim _semaphore;

    public SemaphorePool(int maxCount)
    {
        _semaphore = new SemaphoreSlim(maxCount, maxCount);
    }

    public async Task<IDisposable> AcquireAsync()
    {
        await _semaphore.WaitAsync();
        return new SemaphoreReleaser(_semaphore);
    }

    public async Task<IDisposable> AcquireAsync(TimeSpan timeout)
    {
        var acquired = await _semaphore.WaitAsync(timeout);
        if (!acquired)
        {
            throw new TimeoutException("Failed to acquire semaphore within timeout");
        }
        return new SemaphoreReleaser(_semaphore);
    }

    public int CurrentCount => _semaphore.CurrentCount;

    private class SemaphoreReleaser : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private bool _disposed;

        public SemaphoreReleaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _semaphore.Release();
                _disposed = true;
            }
        }
    }
}
