using Xunit;
using W12CSheets.Client.Helpers;

namespace W12CSheets.Tests.Helpers;

public class DIContainerTests
{
    private interface IService { }
    private class Service : IService { }

    [Fact]
    public void RegisterTransient_ShouldRegister()
    {
        var container = new DIContainer();
        container.RegisterTransient<IService, Service>();
        
        Assert.True(container.IsRegistered<IService>());
    }

    [Fact]
    public void Resolve_ShouldReturnInstance()
    {
        var container = new DIContainer();
        container.RegisterTransient<IService, Service>();
        
        var service = container.Resolve<IService>();
        
        Assert.NotNull(service);
    }

    [Fact]
    public void RegisterSingleton_ShouldReturnSameInstance()
    {
        var container = new DIContainer();
        container.RegisterSingleton<IService, Service>();
        
        var service1 = container.Resolve<IService>();
        var service2 = container.Resolve<IService>();
        
        Assert.Same(service1, service2);
    }
}
