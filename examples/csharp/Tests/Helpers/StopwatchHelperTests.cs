using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class StopwatchHelperTests
{
    [Fact]
    public void Start_ShouldStartTimer()
    {
        var sw = new StopwatchHelper();
        sw.Start();
        
        Assert.True(sw.IsRunning);
    }

    [Fact]
    public void Stop_ShouldReturnElapsed()
    {
        var sw = new StopwatchHelper();
        sw.Start();
        Thread.Sleep(50);
        var elapsed = sw.Stop();
        
        Assert.True(elapsed.TotalMilliseconds >= 50);
    }

    [Fact]
    public void Lap_ShouldRecordLapTime()
    {
        var sw = new StopwatchHelper();
        sw.Start();
        Thread.Sleep(10);
        sw.Lap("Test");
        
        Assert.Equal(1, sw.GetLaps().Count);
    }
}
