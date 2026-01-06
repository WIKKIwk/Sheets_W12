using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class StackHelperTests
{
    [Fact]
    public void Push_ShouldAddItem()
    {
        var stack = new StackHelper<int>();
        stack.Push(1);
        
        Assert.Equal(1, stack.Count());
    }

    [Fact]
    public void Pop_ShouldRemoveItem()
    {
        var stack = new StackHelper<int>();
        stack.Push(1);
        var item = stack.Pop();
        
        Assert.Equal(1, item);
        Assert.Equal(0, stack.Count());
    }

    [Fact]
    public void IsEmpty_ShouldReturnTrue_WhenEmpty()
    {
        var stack = new StackHelper<int>();
        
        Assert.True(stack.IsEmpty());
    }
}
