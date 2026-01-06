using Xunit;
using W12CSheets.Client.Validation;

namespace W12CSheets.Tests.Validation;

public class FluentValidatorTests
{
    private class TestData
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    [Fact]
    public void Validate_ShouldPassWhenValid()
    {
        var validator = new FluentValidator<TestData>();
        validator.RuleFor(x => x.Name, name => !string.IsNullOrEmpty(name), "Name required");
        
        var data = new TestData { Name = "John" };
        var result = validator.Validate(data);
        
        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFailWhenInvalid()
    {
        var validator = new FluentValidator<TestData>();
        validator.RuleFor(x => x.Age, age => age >= 18, "Must be 18+");
        
        var data = new TestData { Age = 15 };
        var result = validator.Validate(data);
        
        Assert.False(result.IsValid);
        Assert.Contains("Must be 18+", result.Errors);
    }
}
