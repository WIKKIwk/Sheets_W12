using Xunit;
using W12CSheets.Client.Extensions;

namespace W12CSheets.Tests.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void Truncate_ShouldLimitLength()
    {
        var text = "Hello World";
        var result = text.Truncate(5);
        
        Assert.Equal(5, result.Length);
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void IsNullOrWhiteSpace_ShouldReturnTrue()
    {
        Assert.True("  ".IsNullOrWhiteSpace());
        Assert.False("test".IsNullOrWhiteSpace());
    }

    [Fact]
    public void ToTitleCase_ShouldCapitalize()
    {
        var result = "hello world".ToTitleCase();
        
        Assert.Equal("Hello World", result);
    }

    [Fact]
    public void Reverse_ShouldReverseString()
    {
        var result = "abc".Reverse();
        
        Assert.Equal("cba", result);
    }

    [Fact]
    public void CountOccurrences_ShouldCount()
    {
        var count = "ababab".CountOccurrences("ab");
        
        Assert.Equal(3, count);
    }
}
