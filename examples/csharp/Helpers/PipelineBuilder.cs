namespace W12CSheets.Client.Helpers;

/// <summary>
/// Pipeline builder for chaining operations
/// </summary>
public class PipelineBuilder<T>
{
    private readonly List<Func<T, T>> _steps = new();

    /// <summary>
    /// Add pipeline step
    /// </summary>
    public PipelineBuilder<T> AddStep(Func<T, T> step)
    {
        _steps.Add(step);
        return this;
    }

    /// <summary>
    /// Execute pipeline
    /// </summary>
    public T Execute(T input)
    {
        var result = input;
        
        foreach (var step in _steps)
        {
            result = step(result);
        }
        
        return result;
    }

    /// <summary>
    /// Clear all steps
    /// </summary>
    public void Clear()
    {
        _steps.Clear();
    }

    /// <summary>
    /// Get step count
    /// </summary>
    public int StepCount => _steps.Count;
}
