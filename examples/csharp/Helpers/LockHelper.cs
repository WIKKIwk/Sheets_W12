namespace W12CSheets.Client.Helpers;

/// <summary>
/// Lock helper for managing locks
/// </summary>
public class LockHelper
{
    private readonly Dictionary<string, SemaphoreSlim> _locks = new();
    private readonly object _lock = new();

    public async Task<IDisposable> AcquireLockAsync(string key)
    {
        SemaphoreSlim semaphore;
        
        lock (_lock)
        {
            if (!_locks.ContainsKey(key))
            {
                _locks[key] = new SemaphoreSlim(1, 1);
            }
            semaphore = _locks[key];
        }

        await semaphore.WaitAsync();
        return new LockReleaser(semaphore);
    }

    public void RemoveLock(string key)
    {
        lock (_lock)
        {
            if (_locks.ContainsKey(key))
            {
                _locks[key].Dispose();
                _locks.Remove(key);
            }
        }
    }

    private class LockReleaser : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;
        private bool _disposed;

        public LockReleaser(SemaphoreSlim semaphore)
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
