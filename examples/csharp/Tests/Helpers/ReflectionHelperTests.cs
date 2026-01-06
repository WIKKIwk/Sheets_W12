using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class ReflectionHelperTests
{
    private class TestClass
    {
        public string Name { get; set; } = "Test";
        public int Value { get; set; } = 42;
    }

    [Fact]
    public void GetPropertyValue_ShouldReturnValue()
    {
        var obj = new TestClass();
        var value = ReflectionHelper.GetPropertyValue(obj, "Name");
        
        Assert.Equal("Test", value);
    }

    [Fact]
    public void SetPropertyValue_ShouldSetValue()
    {
        var obj = new TestClass();
        ReflectionHelper.SetPropertyValue(obj, "Name", "Updated");
        
        Assert.Equal("Updated", obj.Name);
    }

    [Fact]
    public void GetPropertyNames_ShouldReturnAllProperties()
    {
        var names = ReflectionHelper.GetPropertyNames(typeof(TestClass));
        
        Assert.Contains("Name", names);
        Assert.Contains("Value", names);
    }

    [Fact]
    public void HasProperty_ShouldReturnTrue_WhenPropertyExists()
    {
        var hasName = ReflectionHelper.HasProperty(typeof(TestClass), "Name");
        
        Assert.True(hasName);
    }

    [Fact]
    public void CreateInstance_ShouldCreateObject()
    {
        var instance = ReflectionHelper.CreateInstance<TestClass>();
        
        Assert.NotNull(instance);
        Assert.IsType<TestClass>(instance);
    }
}
