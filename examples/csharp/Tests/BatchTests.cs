// Shorthand simple tests for remaining - 17 tests in one batch
using Xunit;

namespace W12CSharts.Tests.Patterns;

// 1. ObserverPattern
public class ObserverPatternTests
{
    [Fact]
    public void Notify_ShouldCallObservers()
    {
        var subject = new W12CSheets.Client.Patterns.ObservableSubject<string>();
        var received = "";
        subject.Attach(new TestObserver(s => received = s));
        subject.Notify("test");
        Assert.Equal("test", received);
    }
    
    class TestObserver : W12CSheets.Client.Patterns.IObserver<string>
    {
        readonly Action<string> _action;
        public TestObserver(Action<string> action) => _action = action;
        public void Update(string data) => _action(data);
    }
}

// 2. MediatorPattern
public class MediatorPatternTests
{
    [Fact]
    public void Send_ShouldNotifySubscribers()
    {
        var mediator = new W12CSheets.Client.Patterns.Mediator();
        var received = "";
        mediator.Subscribe<string>(msg => received = msg);
        mediator.Send("test");
        Assert.Equal("test", received);
    }
}

// 3. CircuitBreaker done above

// 4. RateLimiter done above

// 5. Validation done above

// 6-17: More simple tests

public class ExpressionHelperTests
{
    [Fact]
    public void GetPropertyName_ShouldReturnName()
    {
        var name = W12CSheets.Client.Helpers.ExpressionHelper.GetPropertyName<TestClass, string>(x => x.Name);
        Assert.Equal("Name", name);
    }
    
    class TestClass { public string Name { get; set; } = ""; }
}

public class StringExtensionsTests  
{
    [Fact]
    public void Truncate_ShouldLimitLength()
    {
        var result = "Hello World".Truncate(5);
        Assert.Equal(5, result.Length);
    }
}

public class DateTimeExtensionsTests
{
    [Fact]
    public void IsToday_ShouldReturnTrue()
    {
        Assert.True(DateTime.Now.IsToday());
    }
}

public class CollectionExtensionsTests
{
    [Fact]
    public void IsNullOrEmpty_ShouldReturnTrue()
    {
        List<int>? list = null;
        Assert.True(list.IsNullOrEmpty());
    }
}

public class NumberExtensionsTests
{
    [Fact]
    public void IsEven_ShouldReturnTrue()
    {
        Assert.True(4.IsEven());
    }
}

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetValueOrDefault_ShouldReturnDefault()
    {
        var dict = new Dictionary<string, int>();
        Assert.Equal(10, dict.GetValueOrDefault("key", 10));
    }
}

public class MathHelperTests
{
    [Fact]
    public void Clamp_ShouldClampValue()
    {
        Assert.Equal(5, W12CSheets.Client.Helpers.MathHelper.Clamp(10, 0, 5));
    }
}

public class FileHelperTests
{
    [Fact]
    public void GetFileSizeString_ShouldFormat()
    {
        var size = W12CSheets.Client.Helpers.FileHelper.GetFileSizeString(1024);
        Assert.Contains("KB", size);
    }
}

public class CryptoHelperTests
{
    [Fact]
    public void Hash_ShouldGenerateHash()
    {
        var hash = W12CSheets.Client.Helpers.CryptoHelper.Hash("test");
        Assert.NotEmpty(hash);
    }
}

public class ColorHelperTests
{
    [Fact]
    public void HexToRGB_ShouldConvert()
    {
        var rgb = W12CSheets.Client.Helpers.ColorHelper.HexToRGB("#FFFFFF");
        Assert.Equal((255, 255, 255), rgb);
    }
}

public class EnumHelperTests
{
    [Fact]
    public void GetNames_ShouldReturnNames()
    {
        var names = W12CSheets.Client.Helpers.EnumHelper.GetNames<DayOfWeek>();
        Assert.NotEmpty(names);
    }
}

public class PathHelperTests
{
    [Fact]
    public void Combine_ShouldCombinePaths()
    {
        var path = W12CSheets.Client.Helpers.PathHelper.Combine("a", "b");
        Assert.Contains("a", path);
    }
}

public class ConfigHelperTests
{
    [Fact]
    public void Get_ShouldReturnDefault()
    {
        W12CSheets.Client.Helpers.ConfigHelper.Clear();
        Assert.Equal("default", W12CSheets.Client.Helpers.ConfigHelper.Get("key", "default"));
    }
}

public class CloneHelperTests
{
    [Fact]
    public void DeepClone_ShouldClone()
    {
        var obj = new { Name = "Test" };
        var clone = W12CSheets.Client.Helpers.CloneHelper.DeepClone(obj);
        Assert.NotNull(clone);
    }
}
