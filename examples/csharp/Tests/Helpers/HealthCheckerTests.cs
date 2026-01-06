using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class HealthCheckerTests
{
    [Fact]
    public async Task RegisterCheck_ShouldRegister()
    {
        var checker = new HealthChecker();
        checker.RegisterCheck("db", async () => true);
        
        var report = await checker.CheckHealthAsync();
        
        Assert.Contains("db", report.Checks.Keys);
    }

    [Fact]
    public async Task CheckHealthAsync_ShouldReturnHealthy()
    {
        var checker = new HealthChecker();
        checker.RegisterCheck("test", async () => true);
        
        var report = await checker.CheckHealthAsync();
        
        Assert.True(report.IsHealthy);
    }

    [Fact]
    public async Task CheckHealthAsync_ShouldReturnUnhealthy_OnFailure()
    {
        var checker = new HealthChecker();
        checker.RegisterCheck("fail", async () => false);
        
        var report = await checker.CheckHealthAsync();
        
        Assert.False(report.IsHealthy);
    }
}
