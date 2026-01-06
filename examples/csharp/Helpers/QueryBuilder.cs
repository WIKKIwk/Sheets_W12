namespace W12CSheets.Client.Helpers;

/// <summary>
/// Query builder for constructing queries
/// </summary>
public class QueryBuilder<T>
{
    private List<Func<IEnumerable<T>, IEnumerable<T>>> _operations = new();

    /// <summary>
    /// Add where clause
    /// </summary>
    public QueryBuilder<T> Where(Func<T, bool> predicate)
    {
        _operations.Add(data => data.Where(predicate));
        return this;
    }

    /// <summary>
    /// Add order by clause
    /// </summary>
    public QueryBuilder<T> OrderBy<TKey>(Func<T, TKey> keySelector)
    {
        _operations.Add(data => data.OrderBy(keySelector));
        return this;
    }

    /// <summary>
    /// Add order by descending clause
    /// </summary>
    public QueryBuilder<T> OrderByDescending<TKey>(Func<T, TKey> keySelector)
    {
        _operations.Add(data => data.OrderByDescending(keySelector));
        return this;
    }

    /// <summary>
    /// Add skip clause
    /// </summary>
    public QueryBuilder<T> Skip(int count)
    {
        _operations.Add(data => data.Skip(count));
        return this;
    }

    /// <summary>
    /// Add take clause
    /// </summary>
    public QueryBuilder<T> Take(int count)
    {
        _operations.Add(data => data.Take(count));
        return this;
    }

    /// <summary>
    /// Execute query
    /// </summary>
    public IEnumerable<T> Execute(IEnumerable<T> data)
    {
        var result = data;
        
        foreach (var operation in _operations)
        {
            result = operation(result);
        }
        
        return result;
    }

    /// <summary>
    /// Reset query
    /// </summary>
    public void Reset()
    {
        _operations.Clear();
    }
}
