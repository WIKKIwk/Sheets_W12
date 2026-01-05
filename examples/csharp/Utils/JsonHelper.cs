using System.Text;

namespace W12CSheets.Client.Utils;

/// <summary>
/// JSON helper utilities
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// Pretty print JSON with indentation
    /// </summary>
    public static string PrettyPrint(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return json;

        var indent = 0;
        var sb = new StringBuilder();
        bool inString = false;

        foreach (char c in json)
        {
            if (c == '"' && (sb.Length == 0 || sb[sb.Length - 1] != '\\'))
            {
                inString = !inString;
            }

            if (!inString)
            {
                if (c == '{' || c == '[')
                {
                    sb.Append(c);
                    sb.Append(Environment.NewLine);
                    indent++;
                    sb.Append(new string(' ', indent * 2));
                }
                else if (c == '}' || c == ']')
                {
                    sb.Append(Environment.NewLine);
                    indent--;
                    sb.Append(new string(' ', indent * 2));
                    sb.Append(c);
                }
                else if (c == ',')
                {
                    sb.Append(c);
                    sb.Append(Environment.NewLine);
                    sb.Append(new string(' ', indent * 2));
                }
                else if (c == ':')
                {
                    sb.Append(c);
                    sb.Append(' ');
                }
                else if (!char.IsWhiteSpace(c))
                {
                    sb.Append(c);
                }
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Minify JSON (remove whitespace)
    /// </summary>
    public static string Minify(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return json;

        var sb = new StringBuilder();
        bool inString = false;

        foreach (char c in json)
        {
            if (c == '"' && (sb.Length == 0 || sb[sb.Length - 1] != '\\'))
            {
                inString = !inString;
            }

            if (inString || !char.IsWhiteSpace(c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Validate JSON syntax
    /// </summary>
    public static bool IsValidJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            Newtonsoft.Json.Linq.JToken.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
