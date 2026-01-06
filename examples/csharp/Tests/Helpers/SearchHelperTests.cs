using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class SearchHelperTests
{
    [Fact]
    public void LinearSearch_ShouldFindElement()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var index = SearchHelper.LinearSearch(array, 3);
        
        Assert.Equal(2, index);
    }

    [Fact]
    public void BinarySearch_ShouldFindElement()
    {
        var array = new[] { 1, 2, 3, 4, 5 };
        var index = SearchHelper.BinarySearch(array, 3);
        
        Assert.Equal(2, index);
    }

    [Fact]
    public void FindAll_ShouldFindAllOccurrences()
    {
        var array = new[] { 1, 2, 3, 2, 5 };
        var indices = SearchHelper.FindAll(array, 2);
        
        Assert.Equal(2, indices.Count);
        Assert.Contains(1, indices);
        Assert.Contains(3, indices);
    }

    [Fact]
    public void Any_ShouldReturnTrue_WhenExists()
    {
        var list = new[] { 1, 2, 3 };
        var exists = SearchHelper.Any(list, x => x == 2);
        
        Assert.True(exists);
    }
}
