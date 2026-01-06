using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class SingletonHelperTests
{
    private class TestClass
    {
        public int Value { get; set; }
    }

    [Fact]
    public void Instance_ShouldReturnSameInstance()
    {
        var instance1 = SingletonHelper<TestClass>.Instance;
        var instance2 = SingletonHelper<TestClass>.Instance;
        
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void HasInstance_ShouldReturnTrue_AfterAccess()
    {
        SingletonHelper<TestClass>.Reset();
        var instance = SingletonHelper<TestClass>.Instance;
        
        Assert.True(SingletonHelper<TestClass>.HasInstance);
    }

    [Fact]
    public void Reset_ShouldClearInstance()
    {
        var instance1 = SingletonHelper<TestClass>.Instance;
        SingletonHelper<TestClass>.Reset();
        var instance2 = SingletonHelper<TestClass>.Instance;
        
        Assert.NotSame(instance1, instance2);
    }
}
