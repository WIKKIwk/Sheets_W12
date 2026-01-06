using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessResult()
    {
        var result = Result<int>.Success(42);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Failure_ShouldCreateFailureResult()
    {
        var result = Result<int>.Failure("Error");
        
        Assert.False(result.IsSuccess);
        Assert.Equal("Error", result.Error);
    }

    [Fact]
    public void Match_ShouldCallCorrectHandler()
    {
        var result = Result<int>.Success(42);
        
        var value = result.Match(
            onSuccess: v => v,
            onFailure: e => 0
        );
        
        Assert.Equal(42, value);
    }

    [Fact]
    public void Map_ShouldTransformValue()
    {
        var result = Result<int>.Success(42);
        var mapped = result.Map(x => x.ToString());
        
        Assert.True(mapped.IsSuccess);
        Assert.Equal("42", mapped.Value);
    }
}
