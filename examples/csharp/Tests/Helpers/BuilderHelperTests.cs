using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class BuilderHelperTests
{
    private class TestData
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    [Fact]
    public void Build_ShouldCreateObject()
    {
        var result = BuilderHelper<TestData>.Create()
            .Set("Name", "John")
            .Set("Age", 30)
            .Build();
        
        Assert.NotNull(result);
        Assert.Equal("John", result.Name);
        Assert.Equal(30, result.Age);
    }

    [Fact]
    public void With_ShouldApplyAction()
    {
        var result = BuilderHelper<TestData>.Create()
            .With(x => x.Name = "Jane")
            .Build();
        
        Assert.Equal("Jane", result.Name);
    }
}
