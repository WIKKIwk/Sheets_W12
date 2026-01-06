using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class PathHelperTests
{
    [Fact]
    public void Combine_ShouldCombinePaths()
    {
        var path = PathHelper.Combine("a", "b", "c");
        
        Assert.Contains("a", path);
        Assert.Contains("b", path);
        Assert.Contains("c", path);
    }

    [Fact]
    public void GetExtensionWithoutDot_ShouldRemoveDot()
    {
        var ext = PathHelper.GetExtensionWithoutDot("file.txt");
        
        Assert.Equal("txt", ext);
    }

    [Fact]
    public void ChangeExtension_ShouldChangeExtension()
    {
        var newPath = PathHelper.ChangeExtension("file.txt", ".pdf");
        
        Assert.EndsWith(".pdf", newPath);
    }

    [Fact]
    public void GetFileNameWithoutExtension_ShouldReturnName()
    {
        var name = PathHelper.GetFileNameWithoutExtension("test.txt");
        
        Assert.Equal("test", name);
    }

    [Fact]
    public void IsAbsolutePath_ShouldDetectAbsolutePath()
    {
        var absolute = Path.GetFullPath("test");
        
        Assert.True(PathHelper.IsAbsolutePath(absolute));
    }
}
