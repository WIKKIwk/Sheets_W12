namespace W12CSheets.Client.Patterns;

/// <summary>
/// Unit of Work pattern
/// </summary>
public interface IUnitOfWork : IDisposable
{
    void Commit();
    void Rollback();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly Dictionary<Type, object> _repositories = new();
    private readonly List<Action> _operations = new();
    private bool _committed = false;

    /// <summary>
    /// Register repository
    /// </summary>
    public void RegisterRepository<T>(IRepository<T> repository) where T : class
    {
        _repositories[typeof(T)] = repository;
    }

    /// <summary>
    /// Get repository
    /// </summary>
    public IRepository<T>? GetRepository<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out var repository))
        {
            return repository as IRepository<T>;
        }
        return null;
    }

    /// <summary>
    /// Track operation
    /// </summary>
    public void TrackOperation(Action operation)
    {
        _operations.Add(operation);
    }

    /// <summary>
    /// Commit all changes
    /// </summary>
    public void Commit()
    {
        if (_committed)
        {
            throw new InvalidOperationException("Unit of work already committed");
        }

        try
        {
            foreach (var operation in _operations)
            {
                operation();
            }
            _committed = true;
        }
        catch
        {
            Rollback();
            throw;
        }
    }

    /// <summary>
    /// Rollback all changes
    /// </summary>
    public void Rollback()
    {
        _operations.Clear();
    }

    public void Dispose()
    {
        if (!_committed)
        {
            Rollback();
        }
    }
}
