using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ConfigHelperTests
{
    [Fact]
    public void Set_ShouldStoreValue()
    {
        ConfigHelper.Set("test", "value");
        var result = ConfigHelper.Get("test");
        
        Assert.Equal("value", result);
    }

    [Fact]
    public void Get_ShouldReturnDefault_WhenMissing()
    {
        ConfigHelper.Clear();
        var result = ConfigHelper.Get("missing", "default");
        
        Assert.Equal("default", result);
    }

    [Fact]
    public void GetInt_ShouldParseInteger()
    {
        ConfigHelper.Set("port", "8080");
        var result = ConfigHelper.GetInt("port");
        
        Assert.Equal(8080, result);
    }

    [Fact]
    public void GetBool_ShouldParseBoolean()
    {
        ConfigHelper.Set("enabled", "true");
        var result = ConfigHelper.GetBool("enabled");
        
        Assert.True(result);
    }

    [Fact]
    public void HasKey_ShouldReturnTrue_WhenExists()
    {
        ConfigHelper.Set("key", "value");
        
        Assert.True(ConfigHelper.HasKey("key"));
    }
}
