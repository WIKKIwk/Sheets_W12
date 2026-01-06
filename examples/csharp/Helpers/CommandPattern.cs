namespace W12CSheets.Client.Helpers;

/// <summary>
/// Command pattern helper
/// </summary>
public interface ICommand
{
    void Execute();
    void Undo();
}

public class CommandInvoker
{
    private readonly Stack<ICommand> _history = new();
    private readonly Stack<ICommand> _undoneCommands = new();

    /// <summary>
    /// Execute command
    /// </summary>
    public void Execute(ICommand command)
    {
        command.Execute();
        _history.Push(command);
        _undoneCommands.Clear();
    }

    /// <summary>
    /// Undo last command
    /// </summary>
    public void Undo()
    {
        if (_history.Count == 0)
        {
            return;
        }

        var command = _history.Pop();
        command.Undo();
        _undoneCommands.Push(command);
    }

    /// <summary>
    /// Redo last undone command
    /// </summary>
    public void Redo()
    {
        if (_undoneCommands.Count == 0)
        {
            return;
        }

        var command = _undoneCommands.Pop();
        command.Execute();
        _history.Push(command);
    }

    /// <summary>
    /// Clear history
    /// </summary>
    public void ClearHistory()
    {
        _history.Clear();
        _undoneCommands.Clear();
    }

    /// <summary>
    /// Check if can undo
    /// </summary>
    public bool CanUndo => _history.Count > 0;

    /// <summary>
    /// Check if can redo
    /// </summary>
    public bool CanRedo => _undoneCommands.Count > 0;

    /// <summary>
    /// Get history count
    /// </summary>
    public int HistoryCount => _history.Count;
}
