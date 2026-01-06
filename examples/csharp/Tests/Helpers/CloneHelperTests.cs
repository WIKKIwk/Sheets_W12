using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class CloneHelperTests
{
    private class TestData
    {
        public string Name { get; set; } = "";
        public int Value { get; set; }
    }

    [Fact]
    public void DeepClone_ShouldCreateCopy()
    {
        var original = new TestData { Name = "Test", Value = 42 };
        var clone = CloneHelper.DeepClone(original);
        
        Assert.NotNull(clone);
        Assert.Equal(original.Name, clone!.Name);
        Assert.NotSame(original, clone);
    }

    [Fact]
    public void CloneList_ShouldCloneAllItems()
    {
        var list = new List<TestData>
        {
            new() { Name = "A" },
            new() { Name = "B" }
        };
        
        var cloned = CloneHelper.CloneList(list);
        
        Assert.Equal(2, cloned.Count);
        Assert.NotSame(list[0], cloned[0]);
    }

    [Fact]
    public void CloneDictionary_ShouldCloneItems()
    {
        var dict = new Dictionary<string, TestData>
        {
            ["key1"] = new() { Name = "Test" }
        };
        
        var cloned = CloneHelper.CloneDictionary(dict);
        
        Assert.Single(cloned);
        Assert.NotSame(dict["key1"], cloned["key1"]);
    }
}
