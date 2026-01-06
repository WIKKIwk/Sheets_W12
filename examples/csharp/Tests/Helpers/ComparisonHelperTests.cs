using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ComparisonHelperTests
{
    private class TestData
    {
        public string Name { get; set; } = "";
        public int Value { get; set; }
    }

    [Fact]
    public void DeepEquals_ShouldReturnTrue_WhenEqual()
    {
        var obj1 = new TestData { Name = "Test", Value = 1 };
        var obj2 = new TestData { Name = "Test", Value = 1 };
        
        Assert.True(ComparisonHelper.DeepEquals(obj1, obj2));
    }

    [Fact]
    public void ArraysEqual_ShouldReturnTrue_WhenEqual()
    {
        var arr1 = new[] { 1, 2, 3 };
        var arr2 = new[] { 1, 2, 3 };
        
        Assert.True(ComparisonHelper.ArraysEqual(arr1, arr2));
    }

    [Fact]
    public void FindDifferences_ShouldReturnDifferences()
    {
        var obj1 = new TestData { Name = "Test1", Value = 1 };
        var obj2 = new TestData { Name = "Test2", Value = 2 };
        
        var diffs = ComparisonHelper.FindDifferences(obj1, obj2);
        
        Assert.Equal(2, diffs.Count);
    }
}
