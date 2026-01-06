using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class EnumHelperTests
{
    private enum TestEnum { Value1, Value2, Value3 }

    [Fact]
    public void GetValues_ShouldReturnAllValues()
    {
        var values = EnumHelper.GetValues<TestEnum>();
        
        Assert.Equal(3, values.Length);
    }

    [Fact]
    public void GetNames_ShouldReturnAllNames()
    {
        var names = EnumHelper.GetNames<TestEnum>();
        
        Assert.Contains("Value1", names);
        Assert.Contains("Value2", names);
    }

    [Fact]
    public void Parse_ShouldParseValidValue()
    {
        var result = EnumHelper.Parse<TestEnum>("Value1");
        
        Assert.Equal(TestEnum.Value1, result);
    }

    [Fact]
    public void Parse_ShouldReturnNull_ForInvalid()
    {
        var result = EnumHelper.Parse<TestEnum>("Invalid");
        
        Assert.Null(result);
    }

    [Fact]
    public void ToDictionary_ShouldCreateDictionary()
    {
        var dict = EnumHelper.ToDictionary<TestEnum>();
        
        Assert.Equal(3, dict.Count);
    }
}
