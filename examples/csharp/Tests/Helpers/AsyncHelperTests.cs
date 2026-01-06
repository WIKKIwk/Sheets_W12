using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class AsyncHelperTests
{
    [Fact]
    public void RunSync_ShouldExecuteAsyncMethod()
    {
        var result = AsyncHelper.RunSync(async () =>
        {
            await Task.Delay(10);
            return 42;
        });
        
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task ForEachAsync_ShouldProcessItems()
    {
        var items = new[] { 1, 2, 3 };
        var results = new List<int>();
        
        await AsyncHelper.ForEachAsync(items, 2, async item =>
        {
            await Task.Delay(10);
            results.Add(item);
        });
        
        Assert.Equal(3, results.Count);
    }

    [Fact]
    public async Task WhenAny_ShouldReturnFastest()
    {
        var task1 = Task.Delay(100).ContinueWith(_ => 1);
        var task2 = Task.Delay(10).ContinueWith(_ => 2);
        
        var result = await AsyncHelper.WhenAny(task1, task2);
        
        Assert.Equal(2, result);
    }
}
