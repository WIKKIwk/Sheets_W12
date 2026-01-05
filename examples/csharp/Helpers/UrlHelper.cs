namespace W12CSheets.Client.Helpers;

/// <summary>
/// URL helper utilities
/// </summary>
public static class UrlHelper
{
    /// <summary>
    /// Build query string from parameters
    /// </summary>
    public static string BuildQueryString(Dictionary<string, string> parameters)
    {
        var pairs = parameters.Select(kvp =>$"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}");
        return string.Join("&", pairs);
    }

    /// <summary>
    /// Parse query string to dictionary
    /// </summary>
    public static Dictionary<string, string> ParseQueryString(string queryString)
    {
        var result = new Dictionary<string, string>();
        
        queryString = queryString.TrimStart('?');
        var pairs = queryString.Split('&');
        
        foreach (var pair in pairs)
        {
            var parts = pair.Split('=');
            if (parts.Length == 2)
            {
                var key = Uri.UnescapeDataString(parts[0]);
                var value = Uri.UnescapeDataString(parts[1]);
                result[key] = value;
            }
        }
        
        return result;
    }

    /// <summary>
    /// Combine URL parts
    /// </summary>
    public static string Combine(params string[] parts)
    {
        if (parts.Length == 0)
            return "";

        var result = parts[0].TrimEnd('/');
        
        for (int i = 1; i < parts.Length; i++)
        {
            result += "/" + parts[i].Trim('/');
        }
        
        return result;
    }

    /// <summary>
    /// Get file name from URL
    /// </summary>
    public static string GetFileName(string url)
    {
        try
        {
            var uri = new Uri(url);
            return Path.GetFileName(uri.LocalPath);
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// Check if URL is absolute
    /// </summary>
    public static bool IsAbsolute(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    /// <summary>
    /// Add or update query parameter
    /// </summary>
    public static string AddOrUpdateParameter(string url, string key, string value)
    {
        var uri = new UriBuilder(url);
        var query = ParseQueryString(uri.Query);
        query[key] = value;
        uri.Query = BuildQueryString(query);
        return uri.ToString();
    }

    /// <summary>
    /// Remove query parameter
    /// </summary>
    public static string RemoveParameter(string url, string key)
    {
        var uri = new UriBuilder(url);
        var query = ParseQueryString(uri.Query);
        query.Remove(key);
        uri.Query = BuildQueryString(query);
        return uri.ToString();
    }
}
