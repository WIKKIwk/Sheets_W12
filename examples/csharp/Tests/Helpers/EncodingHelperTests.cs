using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class EncodingHelperTests
{
    [Fact]
    public void ToBase64_ShouldEncodeString()
    {
        var text = "Hello";
        var encoded = EncodingHelper.ToBase64(text);
        
        Assert.NotEmpty(encoded);
    }

    [Fact]
    public void FromBase64_ShouldDecodeString()
    {
        var text = "Hello";
        var encoded = EncodingHelper.ToBase64(text);
        var decoded = EncodingHelper.FromBase64(encoded);
        
        Assert.Equal(text, decoded);
    }

    [Fact]
    public void UrlEncode_ShouldEncodeUrl()
    {
        var url = "hello world";
        var encoded = EncodingHelper.UrlEncode(url);
        
        Assert.Contains("hello", encoded);
    }

    [Fact]
    public void HtmlEncode_ShouldEncodeHtml()
    {
        var html = "<div>test</div>";
        var encoded = EncodingHelper.HtmlEncode(html);
        
        Assert.DoesNotContain("<div>", encoded);
    }
}
