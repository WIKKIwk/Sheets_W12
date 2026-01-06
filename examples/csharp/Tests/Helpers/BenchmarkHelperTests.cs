using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class BenchmarkHelperTests
{
    [Fact]
    public void Benchmark_ShouldMeasureTime()
    {
        var result = BenchmarkHelper.Benchmark(() => Thread.Sleep(10), iterations: 10);
        
        Assert.True(result.TotalTime.TotalMilliseconds > 0);
        Assert.Equal(10, result.Iterations);
    }

    [Fact]
    public async Task BenchmarkAsync_ShouldMeasureAsyncTime()
    {
        var result = await BenchmarkHelper.BenchmarkAsync(
            async () => await Task.Delay(10), 
            iterations: 5);
        
        Assert.True(result.AverageTime.TotalMilliseconds > 0);
    }
}
