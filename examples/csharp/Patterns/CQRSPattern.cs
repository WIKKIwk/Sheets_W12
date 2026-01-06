namespace W12CSheets.Client.Patterns;

/// <summary>
/// CQRS Command and Query interfaces
/// </summary>
public interface ICommand
{
}

public interface ICommand<TResult>
{
}

public interface IQuery<TResult>
{
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task HandleAsync(TCommand command);
}

public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> HandleAsync(TCommand command);
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}

/// <summary>
/// Command/Query dispatcher
/// </summary>
public class Dispatcher
{
    private readonly Dictionary<Type, object> _handlers = new();

    public void RegisterCommandHandler<TCommand>(ICommandHandler<TCommand> handler)
        where TCommand : ICommand
    {
        _handlers[typeof(TCommand)] = handler;
    }

    public void RegisterCommandHandler<TCommand, TResult>(ICommandHandler<TCommand, TResult> handler)
        where TCommand : ICommand<TResult>
    {
        _handlers[typeof(TCommand)] = handler;
    }

    public void RegisterQueryHandler<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler)
        where TQuery : IQuery<TResult>
    {
        _handlers[typeof(TQuery)] = handler;
    }

    public async Task DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        var handler = _handlers[typeof(TCommand)] as ICommandHandler<TCommand>;
        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}");
        }

        await handler.HandleAsync(command);
    }

    public async Task<TResult> DispatchAsync<TCommand, TResult>(TCommand command)
        where TCommand : ICommand<TResult>
    {
        var handler = _handlers[typeof(TCommand)] as ICommandHandler<TCommand, TResult>;
        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TCommand).Name}");
        }

        return await handler.HandleAsync(command);
    }

    public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query)
        where TQuery : IQuery<TResult>
    {
        var handler = _handlers[typeof(TQuery)] as IQueryHandler<TQuery, TResult>;
        if (handler == null)
        {
            throw new InvalidOperationException($"No handler registered for {typeof(TQuery).Name}");
        }

        return await handler.HandleAsync(query);
    }
}
