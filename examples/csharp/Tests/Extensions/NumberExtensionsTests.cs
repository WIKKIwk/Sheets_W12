using Xunit;
using W12CSheets.Client.Extensions;

namespace W12CSheets.Tests.Extensions;

public class NumberExtensionsTests
{
    [Fact]
    public void IsEven_ShouldReturnTrue_ForEvenNumber()
    {
        Assert.True(4.IsEven());
        Assert.False(3.IsEven());
    }

    [Fact]
    public void IsOdd_ShouldReturnTrue_ForOddNumber()
    {
        Assert.True(3.IsOdd());
        Assert.False(4.IsOdd());
    }

    [Fact]
    public void Clamp_ShouldClampValue()
    {
        Assert.Equal(5, 10.Clamp(0, 5));
        Assert.Equal(3, 3.Clamp(0, 5));
    }

    [Fact]
    public void ToPercentage_ShouldFormatAsPercentage()
    {
        var result = 0.75.ToPercentage();
        
        Assert.Contains("%", result);
    }

    [Fact]
    public void IsPositive_ShouldReturnTrue_ForPositive()
    {
        Assert.True(5.IsPositive());
        Assert.False((-5).IsPositive());
    }
}
