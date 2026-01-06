using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class HistogramTests
{
    [Fact]
    public void Record_ShouldAddValue()
    {
        var histogram = new Histogram();
        histogram.Record(10);
        
        var stats = histogram.GetStatistics();
        
        Assert.Equal(1, stats.Count);
        Assert.Equal(10, stats.Min);
    }

    [Fact]
    public void GetPercentile_ShouldCalculateCorrectly()
    {
        var histogram = new Histogram();
        for (int i = 1; i <= 100; i++)
        {
            histogram.Record(i);
        }
        
        var p50 = histogram.GetPercentile(50);
        
        Assert.True(p50 >= 49 && p50 <= 51);
    }

    [Fact]
    public void GetStatistics_ShouldCalculateCorrectly()
    {
        var histogram = new Histogram();
        histogram.Record(1);
        histogram.Record(5);
        histogram.Record(10);
        
        var stats = histogram.GetStatistics();
        
        Assert.Equal(3, stats.Count);
        Assert.Equal(1, stats.Min);
        Assert.Equal(10, stats.Max);
    }
}
