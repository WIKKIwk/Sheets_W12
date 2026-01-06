using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class OptionTests
{
    [Fact]
    public void Some_ShouldCreateWithValue()
    {
        var option = Option<int>.Some(42);
        
        Assert.True(option.HasValue);
        Assert.Equal(42, option.GetValueOrThrow());
    }

    [Fact]
    public void None_ShouldCreateWithoutValue()
    {
        var option = Option<int>.None();
        
        Assert.False(option.HasValue);
    }

    [Fact]
    public void GetValueOrDefault_ShouldReturnDefault_WhenNone()
    {
        var option = Option<int>.None();
        
        Assert.Equal(10, option.GetValueOrDefault(10));
    }

    [Fact]
    public void Map_ShouldTransformValue_WhenSome()
    {
        var option = Option<int>.Some(42);
        var mapped = option.Map(x => x.ToString());
        
        Assert.True(mapped.HasValue);
        Assert.Equal("42", mapped.GetValueOrThrow());
    }

    [Fact]
    public void Match_ShouldCallCorrectHandler()
    {
        var option = Option<int>.Some(42);
        
        var value = option.Match(
            onSome: v => v,
            onNone: () => 0
        );
        
        Assert.Equal(42, value);
    }
}
