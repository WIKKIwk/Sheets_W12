using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class SerializationHelperTests
{
    [Fact]
    public void ToJson_ShouldSerializeObject()
    {
        var obj = new { Name = "Test", Value = 123 };
        var json = SerializationHelper.ToJson(obj);
        
        Assert.Contains("Test", json);
        Assert.Contains("123", json);
    }

    [Fact]
    public void FromJson_ShouldDeserializeObject()
    {
        var json = "{\"Name\":\"Test\",\"Value\":123}";
        var obj = SerializationHelper.FromJson<dynamic>(json);
        
        Assert.NotNull(obj);
    }

    [Fact]
    public void ToJson_WithIndented_ShouldFormatJson()
    {
        var obj = new { Name = "Test" };
        var json = SerializationHelper.ToJson(obj, indented: true);
        
        Assert.Contains("\n", json);
    }
}
