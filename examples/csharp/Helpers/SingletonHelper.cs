namespace W12CSheets.Client.Helpers;

/// <summary>
/// Singleton pattern helper
/// </summary>
public class SingletonHelper<T> where T : class, new()
{
    private static T? _instance;
    private static readonly object _lock = new();

    /// <summary>
    /// Get singleton instance
    /// </summary>
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            
            return _instance;
        }
    }

    /// <summary>
    /// Reset singleton instance
    /// </summary>
    public static void Reset()
    {
        lock (_lock)
        {
            _instance = null;
        }
    }

    /// <summary>
    /// Check if instance exists
    /// </summary>
    public static bool HasInstance => _instance != null;
}
