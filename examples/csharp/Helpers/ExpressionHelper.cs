using System.Linq.Expressions;

namespace W12CSheets.Client.Helpers;

/// <summary>
/// Expression helper for building LINQ expressions
/// </summary>
public static class ExpressionHelper
{
    /// <summary>
    /// Combine predicates with AND
    /// </summary>
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        
        var combined = Expression.AndAlso(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter)
        );
        
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }

    /// <summary>
    /// Combine predicates with OR
    /// </summary>
    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var parameter = Expression.Parameter(typeof(T));
        
        var combined = Expression.OrElse(
            Expression.Invoke(first, parameter),
            Expression.Invoke(second, parameter)
        );
        
        return Expression.Lambda<Func<T, bool>>(combined, parameter);
    }

    /// <summary>
    /// Negate predicate
    /// </summary>
    public static Expression<Func<T, bool>> Not<T>(
        this Expression<Func<T, bool>> expression)
    {
        var parameter = expression.Parameters[0];
        var negated = Expression.Not(expression.Body);
        return Expression.Lambda<Func<T, bool>>(negated, parameter);
    }

    /// <summary>
    /// Get property name from expression
    /// </summary>
    public static string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }
        
        throw new ArgumentException("Expression must be a member expression");
    }
}
