using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class MathHelperTests
{
    [Fact]
    public void Percentage_ShouldCalculatePercentage()
    {
        var result = MathHelper.Percentage(50, 200);
        
        Assert.Equal(25, result);
    }

    [Fact]
    public void Clamp_ShouldClampValue()
    {
        Assert.Equal(5, MathHelper.Clamp(10, 0, 5));
        Assert.Equal(0, MathHelper.Clamp(-5, 0, 5));
    }

    [Fact]
    public void Average_ShouldCalculateAverage()
    {
        var avg = MathHelper.Average(1, 2, 3, 4, 5);
        
        Assert.Equal(3, avg);
    }

    [Fact]
    public void IsPrime_ShouldReturnTrue_ForPrime()
    {
        Assert.True(MathHelper.IsPrime(7));
        Assert.False(MathHelper.IsPrime(8));
    }

    [Fact]
    public void Factorial_ShouldCalculateFactorial()
    {
        Assert.Equal(120, MathHelper.Factorial(5));
    }
}
