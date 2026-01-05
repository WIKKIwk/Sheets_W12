using W12CSheets.Client.Models;

namespace W12CSheets.Client.Utils;

/// <summary>
/// Data transformation utilities
/// </summary>
public static class DataTransformer
{
    /// <summary>
    /// Transpose cells (swap rows and columns)
    /// </summary>
    public static Dictionary<string, Cell> Transpose(Dictionary<string, Cell> cells)
    {
        var transposed = new Dictionary<string, Cell>();

        foreach (var kvp in cells)
        {
            var (col, row) = CellHelper.ParseCellId(kvp.Key);
            var newCellId = CellHelper.GetCellId(row - 1, col + 1);
            transposed[newCellId] = kvp.Value;
        }

        return transposed;
    }

    /// <summary>
    /// Sort cells by column value
    /// </summary>
    public static Dictionary<string, Cell> SortByColumn(Dictionary<string, Cell> cells, int sortColumn, bool ascending = true)
    {
        // Group by rows
        var rows = new Dictionary<int, Dictionary<int, Cell>>();
        
        foreach (var kvp in cells)
        {
            var (col, row) = CellHelper.ParseCellId(kvp.Key);
            if (!rows.ContainsKey(row))
            {
                rows[row] = new Dictionary<int, Cell>();
            }
            rows[row][col] = kvp.Value;
        }

        // Sort rows by the specified column
        var sortedRows = ascending
            ? rows.OrderBy(r => GetCellValue(r.Value, sortColumn))
            : rows.OrderByDescending(r => GetCellValue(r.Value, sortColumn));

        // Rebuild cells with new row numbers
        var result = new Dictionary<string, Cell>();
        int newRow = 1;

        foreach (var row in sortedRows)
        {
            foreach (var cell in row.Value)
            {
                var newCellId = CellHelper.GetCellId(cell.Key, newRow);
                result[newCellId] = cell.Value;
            }
            newRow++;
        }

        return result;
    }

    /// <summary>
    /// Filter rows based on condition
    /// </summary>
    public static Dictionary<string, Cell> FilterRows(Dictionary<string, Cell> cells, Func<Dictionary<int, Cell>, bool> condition)
    {
        // Group by rows
        var rows = new Dictionary<int, Dictionary<int, Cell>>();
        
        foreach (var kvp in cells)
        {
            var (col, row) = CellHelper.ParseCellId(kvp.Key);
            if (!rows.ContainsKey(row))
            {
                rows[row] = new Dictionary<int, Cell>();
            }
            rows[row][col] = kvp.Value;
        }

        // Filter rows
        var filteredRows = rows.Where(r => condition(r.Value)).ToList();

        // Rebuild cells
        var result = new Dictionary<string, Cell>();
        int newRow = 1;

        foreach (var row in filteredRows)
        {
            foreach (var cell in row.Value)
            {
                var newCellId = CellHelper.GetCellId(cell.Key, newRow);
                result[newCellId] = cell.Value;
            }
            newRow++;
        }

        return result;
    }

    /// <summary>
    /// Remove duplicate rows
    /// </summary>
    public static Dictionary<string, Cell> RemoveDuplicates(Dictionary<string, Cell> cells, int keyColumn)
    {
        // Group by rows
        var rows = new Dictionary<int, Dictionary<int, Cell>>();
        
        foreach (var kvp in cells)
        {
            var (col, row) = CellHelper.ParseCellId(kvp.Key);
            if (!rows.ContainsKey(row))
            {
                rows[row] = new Dictionary<int, Cell>();
            }
            rows[row][col] = kvp.Value;
        }

        // Remove duplicates based on key column
        var seen = new HashSet<string>();
        var uniqueRows = new List<Dictionary<int, Cell>>();

        foreach (var row in rows.Values)
        {
            var keyValue = GetCellValue(row, keyColumn);
            if (!seen.Contains(keyValue))
            {
                seen.Add(keyValue);
                uniqueRows.Add(row);
            }
        }

        // Rebuild cells
        var result = new Dictionary<string, Cell>();
        int newRow = 1;

        foreach (var row in uniqueRows)
        {
            foreach (var cell in row)
            {
                var newCellId = CellHelper.GetCellId(cell.Key, newRow);
                result[newCellId] = cell.Value;
            }
            newRow++;
        }

        return result;
    }

    /// <summary>
    /// Convert all values to uppercase
    /// </summary>
    public static Dictionary<string, Cell> ToUpperCase(Dictionary<string, Cell> cells)
    {
        var result = new Dictionary<string, Cell>();
        foreach (var kvp in cells)
        {
            result[kvp.Key] = new Cell
            {
                Value = kvp.Value.Value.ToUpper(),
                Formula = kvp.Value.Formula,
                Format = kvp.Value.Format
            };
        }
        return result;
    }

    /// <summary>
    /// Convert all values to lowercase
    /// </summary>
    public static Dictionary<string, Cell> ToLowerCase(Dictionary<string, Cell> cells)
    {
        var result = new Dictionary<string, Cell>();
        foreach (var kvp in cells)
        {
            result[kvp.Key] = new Cell
            {
                Value = kvp.Value.Value.ToLower(),
                Formula = kvp.Value.Formula,
                Format = kvp.Value.Format
            };
        }
        return result;
    }

    /// <summary>
    /// Trim whitespace from all cell values
    /// </summary>
    public static Dictionary<string, Cell> TrimValues(Dictionary<string, Cell> cells)
    {
        var result = new Dictionary<string, Cell>();
        foreach (var kvp in cells)
        {
            result[kvp.Key] = new Cell
            {
                Value = kvp.Value.Value.Trim(),
                Formula = kvp.Value.Formula,
                Format = kvp.Value.Format
            };
        }
        return result;
    }

    private static string GetCellValue(Dictionary<int, Cell> row, int column)
    {
        return row.ContainsKey(column) ? row[column].Value : "";
    }
}
