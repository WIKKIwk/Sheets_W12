using System.ComponentModel.DataAnnotations;

namespace W12CSheets.Client.Validation;

/// <summary>
/// Custom validation attributes
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class MinLengthAttribute : ValidationAttribute
{
    private readonly int _minLength;

    public MinLengthAttribute(int minLength)
    {
        _minLength = minLength;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str && str.Length < _minLength)
        {
            return new ValidationResult($"Minimum length is {_minLength}");
        }
        return ValidationResult.Success;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class EmailAddressAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, pattern))
            {
                return new ValidationResult("Invalid email address");
            }
        }
        return ValidationResult.Success;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : ValidationAttribute
{
    private readonly double _min;
    private readonly double _max;

    public RangeAttribute(double min, double max)
    {
        _min = min;
        _max = max;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IComparable comparable)
        {
            var doubleValue = Convert.ToDouble(value);
            if (doubleValue < _min || doubleValue > _max)
            {
                return new ValidationResult($"Value must be between {_min} and {_max}");
            }
        }
        return ValidationResult.Success;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class UrlAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string url && !Uri.TryCreate(url, UriKind.Absolute, out _))
        {
            return new ValidationResult("Invalid URL");
        }
        return ValidationResult.Success;
    }
}
