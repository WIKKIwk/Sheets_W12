using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ObjectPoolTests
{
    private class TestObject { }

    [Fact]
    public void Get_ShouldReturnObject()
    {
        var pool = new ObjectPool<TestObject>();
        var obj = pool.Get();
        
        Assert.NotNull(obj);
    }

    [Fact]
    public void Return_ShouldAddToPool()
    {
        var pool = new ObjectPool<TestObject>();
        var obj = pool.Get();
        pool.Return(obj);
        
        var (available, total) = pool.GetStatistics();
        Assert.Equal(1, available);
    }

    [Fact]
    public void Get_ShouldReuseReturnedObject()
    {
        var pool = new ObjectPool<TestObject>();
        var obj1 = pool.Get();
        pool.Return(obj1);
        var obj2 = pool.Get();
        
        Assert.Same(obj1, obj2);
    }
}
