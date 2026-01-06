using Xunit;
using W12CSheets.Client.Extensions;

namespace W12CSheets.Tests.Extensions;

public class ArrayExtensionsTests
{
    [Fact]
    public void Shuffle_ShouldReorderArray()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var shuffled = array.Shuffle();
        
        Assert.Equal(5, shuffled.Length);
    }

    [Fact]
    public void RandomElement_ShouldReturnElement()
    {
        var array = new[] { 1, 2, 3 };
        var element = array.RandomElement();
        
        Assert.Contains(element, array);
    }

    [Fact]
    public void Chunk_ShouldSplitArray()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var chunks = array.Chunk(2);
        
        Assert.Equal(3, chunks.Length);
        Assert.Equal(2, chunks[0].Length);
    }

    [Fact]
    public void RotateLeft_ShouldRotate()
    {
        var array = new[] { 1, 2, 3, 4 };
        var rotated = array.RotateLeft(1);
        
        Assert.Equal(new[] { 2, 3, 4, 1 }, rotated);
    }
}
