using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ThrottleTests
{
    [Fact]
    public void Execute_ShouldExecuteImmediately()
    {
        var throttle = new Throttle(TimeSpan.FromMilliseconds(100));
        var executed = false;
        
        throttle.Execute(() => executed = true);
        
        Assert.True(executed);
    }

    [Fact]
    public void Execute_ShouldThrottle()
    {
        var throttle = new Throttle(TimeSpan.FromMilliseconds(500));
        var count = 0;
        
        throttle.Execute(() => count++);
        throttle.Execute(() => count++);
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void CanExecute_ShouldReturnFalse_WhenThrottled()
    {
        var throttle = new Throttle(TimeSpan.FromMilliseconds(500));
        throttle.Execute(() => { });
        
        Assert.False(throttle.CanExecute);
    }
}
