namespace W12CSheets.Client.Patterns;

/// <summary>
/// Strategy pattern helper
/// </summary>
public interface IStrategy<TInput, TOutput>
{
    TOutput Execute(TInput input);
}

public class StrategyContext<TInput, TOutput>
{
    private IStrategy<TInput, TOutput>? _strategy;

    public void SetStrategy(IStrategy<TInput, TOutput> strategy)
    {
        _strategy = strategy;
    }

    public TOutput? ExecuteStrategy(TInput input)
    {
        return _strategy != null ? _strategy.Execute(input) : default;
    }
}

/// <summary>
/// Example strategies for demonstration
/// </summary>
public class AdditionStrategy : IStrategy<(int, int), int>
{
    public int Execute((int, int) input)
    {
        return input.Item1 + input.Item2;
    }
}

public class MultiplicationStrategy : IStrategy<(int, int), int>
{
    public int Execute((int, int) input)
    {
        return input.Item1 * input.Item2;
    }
}
