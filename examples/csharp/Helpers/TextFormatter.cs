using System.Text;  

namespace W12CSheets.Client.Helpers;

/// <summary>
/// Text formatting helper utilities
/// </summary>
public static class TextFormatter
{
    /// <summary>
    /// Format text as table
    /// </summary>
    public static string FormatAsTable(string[] headers, List<string[]> rows)
    {
        var columnWidths = new int[headers.Length];
        
        // Calculate column widths
        for (int i = 0; i < headers.Length; i++)
        {
            columnWidths[i] = headers[i].Length;
            foreach (var row in rows)
            {
                if (i < row.Length && row[i].Length > columnWidths[i])
                {
                    columnWidths[i] = row[i].Length;
                }
            }
        }
        
        var sb = new StringBuilder();
        
        // Header
        sb.Append("| ");
        for (int i = 0; i < headers.Length; i++)
        {
            sb.Append(headers[i].PadRight(columnWidths[i]));
            sb.Append(" | ");
        }
        sb.AppendLine();
        
        // Separator
        sb.Append("|");
        for (int i = 0; i < headers.Length; i++)
        {
            sb.Append(new string('-', columnWidths[i] + 2));
            sb.Append("|");
        }
        sb.AppendLine();
        
        // Rows
        foreach (var row in rows)
        {
            sb.Append("| ");
            for (int i = 0; i < headers.Length; i++)
            {
                var value = i < row.Length ? row[i] : "";
                sb.Append(value.PadRight(columnWidths[i]));
                sb.Append(" | ");
            }
            sb.AppendLine();
        }
        
        return sb.ToString();
    }

    /// <summary>
    /// Word wrap text to specified width
    /// </summary>
    public static string WordWrap(string text, int maxWidth)
    {
        var lines = new List<string>();
        var words = text.Split(' ');
        var currentLine = "";
        
        foreach (var word in words)
        {
            if ((currentLine + word).Length > maxWidth)
            {
                if (!string.IsNullOrEmpty(currentLine))
                {
                    lines.Add(currentLine.Trim());
                    currentLine = "";
                }
            }
            currentLine += word + " ";
        }
        
        if (!string.IsNullOrEmpty(currentLine))
        {
            lines.Add(currentLine.Trim());
        }
        
        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Center text in specified width
    /// </summary>
    public static string CenterText(string text, int width)
    {
        if (text.Length >= width) return text;
        
        int leftPadding = (width - text.Length) / 2;
        return text.PadLeft(leftPadding + text.Length).PadRight(width);
    }

    /// <summary>
    /// Create progress bar
    /// </summary>
    public static string ProgressBar(double percentage, int width = 50)
    {
        int filledWidth = (int)(width * (percentage / 100.0));
        int emptyWidth = width - filledWidth;
        
        return $"[{new string('█', filledWidth)}{new string('░', emptyWidth)}] {percentage:F1}%";
    }
}
