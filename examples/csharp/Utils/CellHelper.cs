using System.Text.RegularExpressions;

namespace W12CSheets.Client.Utils;

/// <summary>
/// Utility class for cell operations
/// </summary>
public static class CellHelper
{
    private static readonly Regex CellIdRegex = new(@"^([A-Z]+)(\d+)$", RegexOptions.Compiled);

    /// <summary>
    /// Validate cell ID format (e.g., A1, Z999, AA1)
    /// </summary>
    public static bool IsValidCellId(string cellId)
    {
        return CellIdRegex.IsMatch(cellId);
    }

    /// <summary>
    /// Parse cell ID into column and row
    /// </summary>
    public static (int Column, int Row) ParseCellId(string cellId)
    {
        var match = CellIdRegex.Match(cellId);
        if (!match.Success)
            throw new ArgumentException($"Invalid cell ID: {cellId}");

        var column = ColumnToIndex(match.Groups[1].Value);
        var row = int.Parse(match.Groups[2].Value);

        return (column, row);
    }

    /// <summary>
    /// Convert column letter to index (A=0, B=1, ..., Z=25, AA=26)
    /// </summary>
    public static int ColumnToIndex(string column)
    {
        int index = 0;
        foreach (char c in column)
        {
            index = index * 26 + (c - 'A' + 1);
        }
        return index - 1;
    }

    /// <summary>
    /// Convert column index to letter (0=A, 1=B, ..., 25=Z, 26=AA)
    /// </summary>
    public static string IndexToColumn(int index)
    {
        string column = "";
        index++;
        while (index > 0)
        {
            int remainder = (index - 1) % 26;
            column = (char)('A' + remainder) + column;
            index = (index - 1) / 26;
        }
        return column;
    }

    /// <summary>
    /// Create cell ID from column and row
    /// </summary>
    public static string GetCellId(int column, int row)
    {
        return IndexToColumn(column) + row;
    }
}
