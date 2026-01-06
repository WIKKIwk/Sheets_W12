namespace W12CSheets.Client.Helpers;

/// <summary>
/// Reflection helper utilities
/// </summary>
public static class ReflectionHelper
{
    /// <summary>
    /// Get property value by name
    /// </summary>
    public static object? GetPropertyValue(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        return property?.GetValue(obj);
    }

    /// <summary>
    /// Set property value by name
    /// </summary>
    public static void SetPropertyValue(object obj, string propertyName, object? value)
    {
        var property = obj.GetType().GetProperty(propertyName);
        property?.SetValue(obj, value);
    }

    /// <summary>
    /// Get all property names
    /// </summary>
    public static string[] GetPropertyNames(Type type)
    {
        return type.GetProperties().Select(p => p.Name).ToArray();
    }

    /// <summary>
    /// Check if type has property
    /// </summary>
    public static bool HasProperty(Type type, string propertyName)
    {
        return type.GetProperty(propertyName) != null;
    }

    /// <summary>
    /// Create instance of type
    /// </summary>
    public static object? CreateInstance(Type type)
    {
        return Activator.CreateInstance(type);
    }

    /// <summary>
    /// Create instance of generic type
    /// </summary>
    public static T? CreateInstance<T>() where T : class
    {
        return Activator.CreateInstance<T>();
    }

    /// <summary>
    /// Get method by name
    /// </summary>
    public static System.Reflection.MethodInfo? GetMethod(Type type, string methodName)
    {
        return type.GetMethod(methodName);
    }

    /// <summary>
    /// Invoke method
    /// </summary>
    public static object? InvokeMethod(object obj, string methodName, params object[] parameters)
    {
        var method = obj.GetType().GetMethod(methodName);
        return method?.Invoke(obj, parameters);
    }

    /// <summary>
    /// Get all types in assembly
    /// </summary>
    public static Type[] GetTypesInAssembly(System.Reflection.Assembly assembly)
    {
        return assembly.GetTypes();
    }

    /// <summary>
    /// Check if type implements interface
    /// </summary>
    public static bool ImplementsInterface(Type type, Type interfaceType)
    {
        return interfaceType.IsAssignableFrom(type);
    }
}
