using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class MemoryHelperTests
{
    [Fact]
    public void GetMemoryUsageMB_ShouldReturn

PositiveValue()
    {
        var memory = MemoryHelper.GetMemoryUsageMB();
        
        Assert.True(memory > 0);
    }

    [Fact]
    public void ForceGC_ShouldNotThrow()
    {
        var exception = Record.Exception(() => MemoryHelper.ForceGC());
        
        Assert.Null(exception);
    }

    [Fact]
    public void CreateWeakReference_ShouldCreateReference()
    {
        var obj = new object();
        var weakRef = MemoryHelper.CreateWeakReference(obj);
        
        Assert.NotNull(weakRef);
        Assert.True(weakRef.IsAlive);
    }

    [Fact]
    public void GetGeneration_ShouldReturnValidGeneration()
    {
        var obj = new object();
        var generation = MemoryHelper.GetGeneration(obj);
        
        Assert.True(generation >= 0);
    }
}
