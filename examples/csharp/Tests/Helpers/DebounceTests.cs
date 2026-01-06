using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class DebounceTests
{
    [Fact]
    public async Task Execute_ShouldDelayExecution()
    {
        var debounce = new Debounce(TimeSpan.FromMilliseconds(100));
        var executed = false;
        
        debounce.Execute(() => executed = true);
        Assert.False(executed);
        
        await Task.Delay(150);
        Assert.True(executed);
    }

    [Fact]
    public async Task Execute_ShouldResetTimer()
    {
        var debounce = new Debounce(TimeSpan.FromMilliseconds(100));
        var count = 0;
        
        debounce.Execute(() => count++);
        await Task.Delay(50);
        debounce.Execute(() => count++);
        await Task.Delay(150);
        
        Assert.Equal(1, count);
    }

    [Fact]
    public void Cancel_ShouldCancelExecution()
    {
        var debounce = new Debounce(TimeSpan.FromMilliseconds(100));
        var executed = false;
        
        debounce.Execute(() => executed = true);
        debounce.Cancel();
        
        Assert.False(executed);
    }
}
