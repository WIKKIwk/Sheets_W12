using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class QueueHelperTests
{
    [Fact]
    public void Enqueue_ShouldAddItem()
    {
        var queue = new QueueHelper<int>();
        queue.Enqueue(1);
        
        Assert.Equal(1, queue.Count());
    }

    [Fact]
    public void Dequeue_ShouldRemoveItem()
    {
        var queue = new QueueHelper<int>();
        queue.Enqueue(1);
        var item = queue.Dequeue();
        
        Assert.Equal(1, item);
        Assert.Equal(0, queue.Count());
    }

    [Fact]
    public void Peek_ShouldNotRemoveItem()
    {
        var queue = new QueueHelper<int>();
        queue.Enqueue(1);
        var item = queue.Peek();
        
        Assert.Equal(1, item);
        Assert.Equal(1, queue.Count());
    }
}
