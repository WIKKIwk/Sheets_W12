namespace W12CSheets.Client.Helpers;

/// <summary>
/// Memory helper utilities
/// </summary>
public static class MemoryHelper
{
    /// <summary>
    /// Get current process memory usage in MB
    /// </summary>
    public static double GetMemoryUsageMB()
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();
        return process.WorkingSet64 / 1024.0 / 1024.0;
    }

    /// <summary>
    /// Get current process memory usage in bytes
    /// </summary>
    public static long GetMemoryUsageBytes()
    {
        var process = System.Diagnostics.Process.GetCurrentProcess();
        return process.WorkingSet64;
    }

    /// <summary>
    /// Force garbage collection
    /// </summary>
    public static void ForceGC()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }

    /// <summary>
    /// Get total available memory in MB
    /// </summary>
    public static long GetTotalMemoryMB()
    {
        return GC.GetTotalMemory(false) / 1024 / 1024;
    }

    /// <summary>
    /// Check if object is in memory
    /// </summary>
    public static bool IsAlive(WeakReference weakRef)
    {
        return weakRef.IsAlive;
    }

    /// <summary>
    /// Create weak reference
    /// </summary>
    public static WeakReference CreateWeakReference<T>(T target) where T : class
    {
        return new WeakReference(target);
    }

    /// <summary>
    /// Get GC generation of object
    /// </summary>
    public static int GetGeneration(object obj)
    {
        return GC.GetGeneration(obj);
    }

    /// <summary>
    /// Suppress finalization
    /// </summary>
    public static void SuppressFinalize(object obj)
    {
        GC.SuppressFinalize(obj);
    }
}
