using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class LazyHelperTests
{
    [Fact]
    public void Value_ShouldCreateOnlyOnce()
    {
        var count = 0;
        var lazy = new LazyHelper<int>(() => ++count);
        
        var val1 = lazy.Value;
        var val2 = lazy.Value;
        
        Assert.Equal(1, count);
        Assert.Equal(1, val1);
        Assert.Equal(1, val2);
    }

    [Fact]
    public void IsValueCreated_ShouldReturnFalse_Initially()
    {
        var lazy = new LazyHelper<int>(() => 42);
        
        Assert.False(lazy.IsValueCreated);
    }

    [Fact]
    public void Reset_ShouldAllowRecreation()
    {
        var count = 0;
        var lazy = new LazyHelper<int>(() => ++count);
        
        var val1 = lazy.Value;
        lazy.Reset();
        var val2 = lazy.Value;
        
        Assert.Equal(2, count);
    }
}
