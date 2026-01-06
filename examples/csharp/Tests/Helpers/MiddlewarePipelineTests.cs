using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class MiddlewarePipelineTests
{
    [Fact]
    public async Task ExecuteAsync_ShouldCallMiddleware()
    {
        var pipeline = new MiddlewarePipeline<string>();
        var called = false;
        
        pipeline.Use(async (ctx, next) =>
        {
            called = true;
            await next();
        });
        
        await pipeline.ExecuteAsync("test");
        
        Assert.True(called);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldCallInOrder()
    {
        var pipeline = new MiddlewarePipeline<string>();
        var order = new List<int>();
        
        pipeline.Use(async (ctx, next) => { order.Add(1); await next(); });
        pipeline.Use(async (ctx, next) => { order.Add(2); await next(); });
        
        await pipeline.ExecuteAsync("test");
        
        Assert.Equal(new[] { 1, 2 }, order);
    }

    [Fact]
    public void Count_ShouldReturnMiddlewareCount()
    {
        var pipeline = new MiddlewarePipeline<string>();
        pipeline.Use(async (ctx, next) => await next());
        
        Assert.Equal(1, pipeline.Count);
    }
}
