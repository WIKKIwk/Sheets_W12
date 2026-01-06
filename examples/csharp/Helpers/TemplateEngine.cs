namespace W12CSheets.Client.Helpers;

/// <summary>
/// Simple template engine for string templates
/// </summary>
public static class TemplateEngine
{
    /// <summary>
    /// Render template with data
    /// </summary>
    public static string Render(string template, Dictionary<string, string> data)
    {
        var result = template;
        
        foreach (var kvp in data)
        {
            result = result.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);
        }
        
        return result;
    }

    /// <summary>
    /// Render template with object
    /// </summary>
    public static string RenderWithObject<T>(string template, T data)
    {
        var result = template;
        var properties = typeof(T).GetProperties();
        
        foreach (var prop in properties)
        {
            var value = prop.GetValue(data)?.ToString() ?? "";
            result = result.Replace($"{{{{{prop.Name}}}}}", value);
        }
        
        return result;
    }

    /// <summary>
    /// Extract placeholders from template
    /// </summary>
    public static List<string> ExtractPlaceholders(string template)
    {
        var placeholders = new List<string>();
        var pattern = @"\{\{([^}]+)\}\}";
        var matches = System.Text.RegularExpressions.Regex.Matches(template, pattern);
        
        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            placeholders.Add(match.Groups[1].Value);
        }
        
        return placeholders;
    }

    /// <summary>
    /// Check if template has all required data
    /// </summary>
    public static bool HasAllData(string template, Dictionary<string, string> data)
    {
        var placeholders = ExtractPlaceholders(template);
        return placeholders.All(p => data.ContainsKey(p));
    }
}
