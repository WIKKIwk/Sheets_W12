using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ThreadingHelperTests
{
    [Fact]
    public async Task RunAsync_ShouldExecuteAction()
    {
        var executed = false;
        await ThreadingHelper.RunAsync(() => executed = true);
        
        Assert.True(executed);
    }

    [Fact]
    public async Task RunAsync_WithReturn_ShouldReturnValue()
    {
        var result = await ThreadingHelper.RunAsync(() => 42);
        
        Assert.Equal(42, result);
    }

    [Fact]
    public void GetCurrentThreadId_ShouldReturnValidId()
    {
        var threadId = ThreadingHelper.GetCurrentThreadId();
        
        Assert.True(threadId > 0);
    }

    [Fact]
    public void CreateCancellationTokenSource_ShouldCreateWithTimeout()
    {
        var cts = ThreadingHelper.CreateCancellationTokenSource(TimeSpan.FromSeconds(1));
        
        Assert.NotNull(cts);
    }
}
