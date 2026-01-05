using W12CSheets.Client.Models;

namespace W12CSheets.Client.Utils;

/// <summary>
/// Range parser and operations utility
/// </summary>
public static class RangeHelper
{
    /// <summary>
    /// Parse range string (e.g., "A1:B10") into individual cell IDs
    /// </summary>
    public static List<string> ParseRange(string range)
    {
        if (!range.Contains(':'))
        {
            return new List<string> { range };
        }

        var parts = range.Split(':');
        if (parts.Length != 2)
        {
            throw new ArgumentException($"Invalid range format: {range}");
        }

        var (startCol, startRow) = CellHelper.ParseCellId(parts[0]);
        var (endCol, endRow) = CellHelper.ParseCellId(parts[1]);

        var cells = new List<string>();
        for (int row = startRow; row <= endRow; row++)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                cells.Add(CellHelper.GetCellId(col, row));
            }
        }

        return cells;
    }

    /// <summary>
    /// Get range dimensions (width and height)
    /// </summary>
    public static (int Width, int Height) GetRangeDimensions(string range)
    {
        var parts = range.Split(':');
        if (parts.Length != 2)
        {
            return (1, 1);
        }

        var (startCol, startRow) = CellHelper.ParseCellId(parts[0]);
        var (endCol, endRow) = CellHelper.ParseCellId(parts[1]);

        return (endCol - startCol + 1, endRow - startRow + 1);
    }

    /// <summary>
    /// Check if cell is within range
    /// </summary>
    public static bool IsCellInRange(string cellId, string range)
    {
        var parts = range.Split(':');
        if (parts.Length != 2)
        {
            return cellId == range;
        }

        var (cellCol, cellRow) = CellHelper.ParseCellId(cellId);
        var (startCol, startRow) = CellHelper.ParseCellId(parts[0]);
        var (endCol, endRow) = CellHelper.ParseCellId(parts[1]);

        return cellCol >= startCol && cellCol <= endCol &&
               cellRow >= startRow && cellRow <= endRow;
    }

    /// <summary>
    /// Merge multiple ranges into one
    /// </summary>
    public static string MergeRanges(params string[] ranges)
    {
        if (ranges.Length == 0)
            return "";

        if (ranges.Length == 1)
            return ranges[0];

        int minCol = int.MaxValue, minRow = int.MaxValue;
        int maxCol = int.MinValue, maxRow = int.MinValue;

        foreach (var range in ranges)
        {
            var cells = ParseRange(range);
            foreach (var cell in cells)
            {
                var (col, row) = CellHelper.ParseCellId(cell);
                minCol = Math.Min(minCol, col);
                minRow = Math.Min(minRow, row);
                maxCol = Math.Max(maxCol, col);
                maxRow = Math.Max(maxRow, row);
            }
        }

        var startCell = CellHelper.GetCellId(minCol, minRow);
        var endCell = CellHelper.GetCellId(maxCol, maxRow);

        return $"{startCell}:{endCell}";
    }

    /// <summary>
    /// Split range into smaller ranges (useful for batching)
    /// </summary>
    public static List<string> SplitRange(string range, int maxCellsPerRange)
    {
        var cells = ParseRange(range);
        var ranges = new List<string>();

        for (int i = 0; i < cells.Count; i += maxCellsPerRange)
        {
            var batch = cells.Skip(i).Take(maxCellsPerRange).ToList();
            if (batch.Count == 1)
            {
                ranges.Add(batch[0]);
            }
            else
            {
                ranges.Add($"{batch.First()}:{batch.Last()}");
            }
        }

        return ranges;
    }
}
