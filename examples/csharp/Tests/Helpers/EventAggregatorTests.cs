using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class EventAggregatorTests
{
    private class TestEvent
    {
        public string Message { get; set; } = "";
    }

    [Fact]
    public void Subscribe_ShouldReceiveEvent()
    {
        var aggregator = new EventAggregator();
        string? receivedMessage = null;
        
        aggregator.Subscribe<TestEvent>(e => receivedMessage = e.Message);
        aggregator.Publish(new TestEvent { Message = "Test" });
        
        Assert.Equal("Test", receivedMessage);
    }

    [Fact]
    public void Unsubscribe_ShouldNotReceiveEvent()
    {
        var aggregator = new EventAggregator();
        var count = 0;
        Action<TestEvent> handler = e => count++;
        
        aggregator.Subscribe(handler);
        aggregator.Unsubscribe(handler);
        aggregator.Publish(new TestEvent());
        
        Assert.Equal(0, count);
    }

    [Fact]
    public void GetSubscriberCount_ShouldReturnCorrectCount()
    {
        var aggregator = new EventAggregator();
        aggregator.Subscribe<TestEvent>(e => { });
        
        Assert.Equal(1, aggregator.GetSubscriberCount<TestEvent>());
    }
}
