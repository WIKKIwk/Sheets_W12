using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class RateLimiterTests
{
    [Fact]
    public void TryAcquire_ShouldReturnTrue_InitialRequest()
    {
        var limiter = new RateLimiter(10, TimeSpan.FromSeconds(1));
        
        Assert.True(limiter.TryAcquire());
    }

    [Fact]
    public void TryAcquire_ShouldReturnFalse_WhenLimitExceeded()
    {
        var limiter = new RateLimiter(2, TimeSpan.FromSeconds(10));
        
        limiter.TryAcquire();
        limiter.TryAcquire();
        
        Assert.False(limiter.TryAcquire());
    }

    [Fact]
    public void RemainingRequests_ShouldDecrease()
    {
        var limiter = new RateLimiter(5, TimeSpan.FromSeconds(1));
        limiter.TryAcquire();
        
        Assert.Equal(4, limiter.RemainingRequests);
    }
}
