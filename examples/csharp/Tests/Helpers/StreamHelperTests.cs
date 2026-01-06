using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class StreamHelperTests
{
    [Fact]
    public void ReadAllBytes_ShouldReadStream()
    {
        var data = new byte[] { 1, 2, 3, 4, 5 };
        using var stream = new MemoryStream(data);
        
        var result = StreamHelper.ReadAllBytes(stream);
        
        Assert.Equal(data, result);
    }

    [Fact]
    public void ReadAllText_ShouldReadTextStream()
    {
        var text = "Hello World";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(text));
        
        var result = StreamHelper.ReadAllText(stream);
        
        Assert.Equal(text, result);
    }

    [Fact]
    public void StringToStream_ShouldConvertString()
    {
        var text = "Test String";
        using var stream = StreamHelper.StringToStream(text);
        
        Assert.NotNull(stream);
        Assert.True(stream.Length > 0);
    }

    [Fact]
    public void CopyStream_ShouldCopyData()
    {
        using var source = new MemoryStream(new byte[] { 1, 2, 3 });
        using var destination = new MemoryStream();
        
        StreamHelper.CopyStream(source, destination);
        
        Assert.Equal(3, destination.Length);
    }
}
