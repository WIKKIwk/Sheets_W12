using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class SortHelperTests
{
    [Fact]
    public void BubbleSort_ShouldSortArray()
    {
        var array = new[] { 3, 1, 4, 1, 5, 9, 2, 6 };
        var sorted = SortHelper.BubbleSort(array);
        
        Assert.Equal(new[] { 1, 1, 2, 3, 4, 5, 6, 9 }, sorted);
    }

    [Fact]
    public void QuickSort_ShouldSortArray()
    {
        var array = new[] { 3, 1, 4, 1, 5, 9, 2, 6 };
        var sorted = SortHelper.QuickSort(array);
        
        Assert.Equal(new[] { 1, 1, 2, 3, 4, 5, 6, 9 }, sorted);
    }

    [Fact]
    public void SortByValue_ShouldSortDictionary()
    {
        var dict = new Dictionary<string, int>
        {
            ["c"] = 3,
            ["a"] = 1,
            ["b"] = 2
        };
        
        var sorted = SortHelper.SortByValue(dict);
        
        Assert.Equal(1, sorted.First().Value);
    }
}
