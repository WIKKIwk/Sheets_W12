using Xunit;
using W12CSheets.Client.Extensions;

namespace W12CSheets.Tests.Extensions;

public class TaskExtensionsTests
{
    [Fact]
    public async Task WithTimeout_ShouldCompleteSuccessfully()
    {
        var task = Task.FromResult(42);
        var result = await task.WithTimeout(TimeSpan.FromSeconds(1));
        
        Assert.Equal(42, result);
    }

    [Fact]
    public async Task IgnoreExceptions_ShouldNotThrow()
    {
        var task = Task.Run(() => throw new Exception());
        
        var exception = await Record.ExceptionAsync(async () => 
            await task.IgnoreExceptions());
        
        Assert.Null(exception);
    }
}
