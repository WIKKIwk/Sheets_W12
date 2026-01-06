using Xunit;
using W12CSheets.Client.Extensions;

namespace W12CSheets.Tests.Extensions;

public class CollectionExtensionsTests
{
    [Fact]
    public void IsNullOrEmpty_ShouldReturnTrue_WhenNull()
    {
        List<int>? list = null;
        
        Assert.True(list.IsNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_ShouldReturnTrue_WhenEmpty()
    {
        var list = new List<int>();
        
        Assert.True(list.IsNullOrEmpty());
    }

    [Fact]
    public void Chunk_ShouldSplitCollection()
    {
        var list = new[] { 1, 2, 3, 4, 5 };
        var chunks = list.Chunk(2);
        
        Assert.Equal(3, chunks.Count());
    }

    [Fact]
    public void Shuffle_ShouldReorderCollection()
    {
        var list = new[] { 1, 2, 3, 4, 5 };
        var shuffled = list.Shuffle();
        
        Assert.Equal(5, shuffled.Count());
    }

    [Fact]
    public void Random_ShouldReturnElement()
    {
        var list = new[] { 1, 2, 3 };
        var random = list.Random();
        
        Assert.Contains(random, list);
    }
}
