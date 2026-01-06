namespace W12CSheets.Client.Patterns;

/// <summary>
/// Repository pattern base
/// </summary>
public interface IRepository<T> where T : class
{
    T? GetById(string id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(string id);
}

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected readonly List<T> _data = new();
    protected readonly Func<T, string> _idSelector;

    protected Repository(Func<T, string> idSelector)
    {
        _idSelector = idSelector;
    }

    public virtual T? GetById(string id)
    {
        return _data.FirstOrDefault(x => _idSelector(x) == id);
    }

    public virtual IEnumerable<T> GetAll()
    {
        return _data.ToList();
    }

    public virtual void Add(T entity)
    {
        _data.Add(entity);
    }

    public virtual void Update(T entity)
    {
        var id = _idSelector(entity);
        var existing = GetById(id);
        
        if (existing!= null)
        {
            var index = _data.IndexOf(existing);
            _data[index] = entity;
        }
    }

    public virtual void Delete(string id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            _data.Remove(entity);
        }
    }
}

/// <summary>
/// Generic in-memory repository
/// </summary>
public class InMemoryRepository<T> : Repository<T> where T : class
{
    public InMemoryRepository(Func<T, string> idSelector) : base(idSelector)
    {
    }
}
