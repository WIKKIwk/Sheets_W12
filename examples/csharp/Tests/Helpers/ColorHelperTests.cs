using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ColorHelperTests
{
    [Fact]
    public void HexToRGB_ShouldConvertWhite()
    {
        var (r, g, b) = ColorHelper.HexToRGB("#FFFFFF");
        
        Assert.Equal(255, r);
        Assert.Equal(255, g);
        Assert.Equal(255, b);
    }

    [Fact]
    public void RGBToHex_ShouldConvertToHex()
    {
        var hex = ColorHelper.RGBToHex(255, 0, 0);
        
        Assert.Equal("#FF0000", hex.ToUpper());
    }

    [Fact]
    public void GetBrightness_ShouldCalculateBrightness()
    {
        var brightness = ColorHelper.GetBrightness(255, 255, 255);
        
        Assert.True(brightness > 200);
    }

    [Fact]
    public void IsDark_ShouldReturnFalse_ForWhite()
    {
        Assert.False(ColorHelper.IsDark(255, 255, 255));
    }

    [Fact]
    public void RandomColor_ShouldGenerateHex()
    {
        var color = ColorHelper.RandomColor();
        
        Assert.StartsWith("#", color);
    }
}
