using W12CSheets.Client.Models;

namespace W12CSheets.Client.Utils;

/// <summary>
/// CSV export/import utilities
/// </summary>
public static class CsvHelper
{
    /// <summary>
    /// Export cells to CSV format
    /// </summary>
    public static string ExportToCsv(Dictionary<string, Cell> cells, int maxRow = 100, int maxCol = 26)
    {
        var lines = new List<string>();

        for (int row = 1; row <= maxRow; row++)
        {
            var values = new List<string>();
            for (int col = 0; col < maxCol; col++)
            {
                var cellId = CellHelper.GetCellId(col, row);
                var cell = cells.GetValueOrDefault(cellId);
                values.Add(EscapeCsvValue(cell?.Value ?? ""));
            }
            lines.Add(string.Join(",", values));
        }

        return string.Join(Environment.NewLine, lines);
    }

    /// <summary>
    /// Import CSV to cells
    /// </summary>
    public static Dictionary<string, Cell> ImportFromCsv(string csvContent)
    {
        var cells = new Dictionary<string, Cell>();
        var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        for (int rowIndex = 0; rowIndex < lines.Length; rowIndex++)
        {
            var values = ParseCsvLine(lines[rowIndex]);
            for (int colIndex = 0; colIndex < values.Length; colIndex++)
            {
                var cellId = CellHelper.GetCellId(colIndex, rowIndex + 1);
                cells[cellId] = new Cell { Value = values[colIndex] };
            }
        }

        return cells;
    }

    private static string EscapeCsvValue(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }

    private static string[] ParseCsvLine(string line)
    {
        var values = new List<string>();
        var currentValue = "";
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentValue += '"';
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(currentValue);
                currentValue = "";
            }
            else
            {
                currentValue += c;
            }
        }

        values.Add(currentValue);
        return values.ToArray();
    }
}
