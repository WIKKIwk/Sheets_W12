using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class FileHelperTests
{
    [Fact]
    public void GetFileSizeString_ShouldFormatBytes()
    {
        Assert.Contains("B", FileHelper.GetFileSizeString(100));
    }

    [Fact]
    public void GetFileSizeString_ShouldFormatKB()
    {
        var size = FileHelper.GetFileSizeString(2048);
        
        Assert.Contains("KB", size);
    }

    [Fact]
    public void GetFileSizeString_ShouldFormatMB()
    {
        var size = FileHelper.GetFileSizeString(2097152);
        
        Assert.Contains("MB", size);
    }

    [Fact]
    public void EnsureDirectoryExists_ShouldCreateDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        FileHelper.EnsureDirectoryExists(path);
        
        Assert.True(Directory.Exists(path));
        
        Directory.Delete(path);
    }
}
