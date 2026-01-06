using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class TemplateEngineTests
{
    [Fact]
    public void Render_ShouldReplacePlaceholders()
    {
        var template = "Hello {{name}}!";
        var data = new Dictionary<string, string> { ["name"] = "World" };
        
        var result = TemplateEngine.Render(template, data);
        
        Assert.Equal("Hello World!", result);
    }

    [Fact]
    public void ExtractPlaceholders_ShouldFindAll()
    {
        var template = "{{first}} and {{second}}";
        
        var placeholders = TemplateEngine.ExtractPlaceholders(template);
        
        Assert.Equal(2, placeholders.Count);
        Assert.Contains("first", placeholders);
        Assert.Contains("second", placeholders);
    }

    [Fact]
    public void HasAllData_ShouldReturnTrue_WhenComplete()
    {
        var template = "{{name}}";
        var data = new Dictionary<string, string> { ["name"] = "Test" };
        
        Assert.True(TemplateEngine.HasAllData(template, data));
    }
}
