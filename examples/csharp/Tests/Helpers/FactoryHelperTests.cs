using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class FactoryHelperTests
{
    private class Product
    {
        public string Name { get; set; } = "";
    }

    [Fact]
    public void Register_ShouldRegisterFactory()
    {
        var factory = new FactoryHelper<string, Product>();
        factory.Register("test", () => new Product { Name = "Test" });
        
       Assert.True(factory.IsRegistered("test"));
    }

    [Fact]
    public void Create_ShouldCreateInstance()
    {
        var factory = new FactoryHelper<string, Product>();
        factory.Register("test", () => new Product { Name = "Test" });
        
        var product = factory.Create("test");
        
        Assert.NotNull(product);
        Assert.Equal("Test", product!.Name);
    }

    [Fact]
    public void Unregister_ShouldRemoveFactory()
    {
        var factory = new FactoryHelper<string, Product>();
        factory.Register("test", () => new Product());
        factory.Unregister("test");
        
        Assert.False(factory.IsRegistered("test"));
    }
}
