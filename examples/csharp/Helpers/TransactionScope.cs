namespace W12CSheets.Client.Helpers;

/// <summary>
/// Transaction scope for managing transactions
/// </summary>
public class TransactionScope : IDisposable
{
    private bool _committed;
    private readonly List<Action> _commitActions = new();
    private readonly List<Action> _rollbackActions = new();

    public void OnCommit(Action action)
    {
        _commitActions.Add(action);
    }

    public void OnRollback(Action action)
    {
        _rollbackActions.Add(action);
    }

    public void Commit()
    {
        if (_committed)
        {
            throw new InvalidOperationException("Transaction already committed");
        }

        foreach (var action in _commitActions)
        {
            action();
        }

        _committed = true;
    }

    public void Rollback()
    {
        foreach (var action in _rollbackActions)
        {
            action();
        }
    }

    public void Dispose()
    {
        if (!_committed)
        {
            Rollback();
        }
    }
}
