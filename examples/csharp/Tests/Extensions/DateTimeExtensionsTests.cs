using Xunit;
using W12CSheets.Client.Extensions;

namespace W12CSheets.Tests.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void IsToday_ShouldReturnTrue_ForToday()
    {
        var today = DateTime.Now;
        
        Assert.True(today.IsToday());
    }

    [Fact]
    public void IsPast_ShouldReturnTrue_ForPastDate()
    {
        var pastDate = DateTime.Now.AddDays(-1);
        
        Assert.True(pastDate.IsPast());
    }

    [Fact]
    public void IsFuture_ShouldReturnTrue_ForFutureDate()
    {
        var futureDate = DateTime.Now.AddDays(1);
        
        Assert.True(futureDate.IsFuture());
    }

    [Fact]
    public void GetAge_ShouldCalculateAge()
    {
        var birthDate = DateTime.Now.AddYears(-30);
        var age = birthDate.GetAge();
        
        Assert.Equal(30, age);
    }

    [Fact]
    public void StartOfDay_ShouldZeroTime()
    {
        var date = new DateTime(2024, 1, 1, 15, 30, 45);
        var start = date.StartOfDay();
        
        Assert.Equal(0, start.Hour);
        Assert.Equal(0, start.Minute);
    }
}
