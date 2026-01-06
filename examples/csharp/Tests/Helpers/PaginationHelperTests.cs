using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class PaginationHelperTests
{
    [Fact]
    public void Paginate_ShouldReturnCorrectPage()
    {
        var data = Enumerable.Range(1, 100);
        var result = PaginationHelper.Paginate(data, 1, 10);
        
        Assert.Equal(10, result.Items.Count);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.TotalPages);
    }

    [Fact]
    public void Paginate_ShouldHaveCorrectNavigation()
    {
        var data = Enumerable.Range(1, 30);
        var result = PaginationHelper.Paginate(data, 2, 10);
        
        Assert.True(result.HasPreviousPage);
        Assert.True(result.HasNextPage);
    }

    [Fact]
    public void CalculateTotalPages_ShouldBeCorrect()
    {
        var totalPages = PaginationHelper.CalculateTotalPages(25, 10);
        
        Assert.Equal(3, totalPages);
    }
}
