using Xunit;
using W12CSheets.Client.Extensions;

namespace W12CSheets.Tests.Extensions;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetValueOrDefault_ShouldReturnValue()
    {
        var dict = new Dictionary<string, int> { ["key"] = 42 };
        
        Assert.Equal(42, dict.GetValueOrDefault("key", 0));
    }

    [Fact]
    public void GetValueOrDefault_ShouldReturn Default_WhenMissing()
    {
        var dict = new Dictionary<string, int>();
        
        Assert.Equal(10, dict.GetValueOrDefault("missing", 10));
    }

    [Fact]
    public void AddOrUpdate_ShouldUpdateExisting()
    {
        var dict = new Dictionary<string, int> { ["key"] = 1 };
        dict.AddOrUpdate("key", 2);
        
        Assert.Equal(2, dict["key"]);
    }

    [Fact]
    public void ToQueryString_ShouldBuildQueryString()
    {
        var dict = new Dictionary<string, string>
        {
            ["name"] = "test",
            ["age"] = "30"
        };
        
        var query = dict.ToQueryString();
        
        Assert.Contains("name=test", query);
        Assert.Contains("age=30", query);
    }
}
