using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class CircuitBreakerTests
{
    [Fact]
    public void Execute_ShouldExecuteAction()
    {
        var cb = new CircuitBreaker(3);
        var result = cb.Execute(() => 42);
        
        Assert.Equal(42, result);
    }

    [Fact]
    public void Execute_ShouldOpenAfterThreshold()
    {
        var cb = new CircuitBreaker(2);
        
        for (int i = 0; i < 2; i++)
        {
            try { cb.Execute<int>(() => throw new Exception()); }
            catch { }
        }
        
        Assert.Equal(CircuitState.Open, cb.State);
    }

    [Fact]
    public void Reset_ShouldCloseCircuit()
    {
        var cb = new CircuitBreaker(1);
        try { cb.Execute<int>(() => throw new Exception()); }
        catch { }
        
        cb.Reset();
        
        Assert.Equal(CircuitState.Closed, cb.State);
    }
}
