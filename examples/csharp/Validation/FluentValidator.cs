namespace W12CSheets.Client.Validation;

/// <summary>
/// Fluent validation builder
/// </summary>
public class FluentValidator<T>
{
    private readonly List<Func<T, ValidationResult>> _rules = new();

    /// <summary>
    /// Add validation rule
    /// </summary>
    public FluentValidator<T> RuleFor<TProp>(
        Func<T, TProp> propertySelector,
        Func<TProp, bool> predicate,
        string errorMessage)
    {
        _rules.Add(obj =>
        {
            var value = propertySelector(obj);
            return predicate(value)
                ? ValidationResult.Success()
                : ValidationResult.Failure(errorMessage);
        });
        
        return this;
    }

    /// <summary>
    /// Validate object
    /// </summary>
    public ValidationResult Validate(T obj)
    {
        var errors = new List<string>();
        
        foreach (var rule in _rules)
        {
            var result = rule(obj);
            if (!result.IsValid)
            {
                errors.AddRange(result.Errors);
            }
        }
        
        return errors.Count == 0
            ? ValidationResult.Success()
            : ValidationResult.Failure(errors.ToArray());
    }
}

public class ValidationResult
{
    public bool IsValid { get; private set; }
    public string[] Errors { get; private set; }

    private ValidationResult(bool isValid, params string[] errors)
    {
        IsValid = isValid;
        Errors = errors;
    }

    public static ValidationResult Success() => new ValidationResult(true);
    public static ValidationResult Failure(params string[] errors) => new ValidationResult(false, errors);
}
