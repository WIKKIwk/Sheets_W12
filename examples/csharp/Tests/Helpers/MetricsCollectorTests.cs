using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class MetricsCollectorTests
{
    [Fact]
    public void IncrementCounter_ShouldIncrement()
    {
        var metrics = new MetricsCollector();
        metrics.IncrementCounter("test", 5);
        
        Assert.Equal(5, metrics.GetCounter("test"));
    }

    [Fact]
    public void RecordGauge_ShouldRecordValue()
    {
        var metrics = new MetricsCollector();
        metrics.RecordGauge("cpu", 75.5);
        
        var stats = metrics.GetGaugeStatistics("cpu");
        
        Assert.NotNull(stats);
        Assert.Equal(75.5, stats!.Latest);
    }

    [Fact]
    public void GetSnapshot_ShouldReturnAllMetrics()
    {
        var metrics = new MetricsCollector();
        metrics.IncrementCounter("requests");
        metrics.RecordGauge("memory", 100);
        
        var snapshot = metrics.GetSnapshot();
        
        Assert.Contains("requests", snapshot.Counters.Keys);
    }
}
