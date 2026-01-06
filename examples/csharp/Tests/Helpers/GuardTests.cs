using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class GuardTests
{
    [Fact]
    public void AgainstNull_ShouldThrow_WhenNull()
    {
        string? value = null;
        
        Assert.Throws<ArgumentNullException>(() => 
            Guard.AgainstNull(value, nameof(value)));
    }

    [Fact]
    public void AgainstNullOrEmpty_ShouldThrow_WhenEmpty()
    {
        Assert.Throws<ArgumentException>(() => 
            Guard.AgainstNullOrEmpty("", "value"));
    }

    [Fact]
    public void AgainstNegative_ShouldThrow_WhenNegative()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            Guard.AgainstNegative(-1, "value"));
    }

    [Fact]
    public void AgainstOutOfRange_ShouldThrow_WhenOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            Guard.AgainstOutOfRange(10, 0, 5, "value"));
    }

    [Fact]
    public void AgainstEmptyCollection_ShouldThrow_WhenEmpty()
    {
        var empty = new List<int>();
        
        Assert.Throws<ArgumentException>(() => 
            Guard.AgainstEmptyCollection(empty, "collection"));
    }
}
