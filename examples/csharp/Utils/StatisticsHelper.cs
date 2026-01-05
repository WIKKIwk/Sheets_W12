using W12CSheets.Client.Models;

namespace W12CSheets.Client.Utils;

/// <summary>
/// Statistical analysis utilities for spreadsheet data
/// </summary>
public static class StatisticsHelper
{
    /// <summary>
    /// Calculate sum of numeric cell values
    /// </summary>
    public static double Sum(IEnumerable<Cell> cells)
    {
        return cells
            .Where(c => double.TryParse(c.Value, out _))
            .Sum(c => double.Parse(c.Value));
    }

    /// <summary>
    /// Calculate average of numeric cell values
    /// </summary>
    public static double Average(IEnumerable<Cell> cells)
    {
        var numericCells = cells
            .Where(c => double.TryParse(c.Value, out _))
            .Select(c => double.Parse(c.Value))
            .ToList();

        if (numericCells.Count == 0)
            return 0;

        return numericCells.Average();
    }

    /// <summary>
    /// Find minimum value
    /// </summary>
    public static double Min(IEnumerable<Cell> cells)
    {
        var numericCells = cells
            .Where(c => double.TryParse(c.Value, out _))
            .Select(c => double.Parse(c.Value))
            .ToList();

        if (numericCells.Count == 0)
            return 0;

        return numericCells.Min();
    }

    /// <summary>
    /// Find maximum value
    /// </summary>
    public static double Max(IEnumerable<Cell> cells)
    {
        var numericCells = cells
            .Where(c => double.TryParse(c.Value, out _))
            .Select(c => double.Parse(c.Value))
            .ToList();

        if (numericCells.Count == 0)
            return 0;

        return numericCells.Max();
    }

    /// <summary>
    /// Count non-empty cells
    /// </summary>
    public static int Count(IEnumerable<Cell> cells)
    {
        return cells.Count(c => !string.IsNullOrWhiteSpace(c.Value));
    }

    /// <summary>
    /// Count cells with numeric values
    /// </summary>
    public static int CountNumeric(IEnumerable<Cell> cells)
    {
        return cells.Count(c => double.TryParse(c.Value, out _));
    }

    /// <summary>
    /// Calculate median value
    /// </summary>
    public static double Median(IEnumerable<Cell> cells)
    {
        var numericCells = cells
            .Where(c => double.TryParse(c.Value, out _))
            .Select(c => double.Parse(c.Value))
            .OrderBy(x => x)
            .ToList();

        if (numericCells.Count == 0)
            return 0;

        int middle = numericCells.Count / 2;
        if (numericCells.Count % 2 == 0)
        {
            return (numericCells[middle - 1] + numericCells[middle]) / 2.0;
        }
        else
        {
            return numericCells[middle];
        }
    }

    /// <summary>
    /// Calculate standard deviation
    /// </summary>
    public static double StandardDeviation(IEnumerable<Cell> cells)
    {
        var numericCells = cells
            .Where(c => double.TryParse(c.Value, out _))
            .Select(c => double.Parse(c.Value))
            .ToList();

        if (numericCells.Count == 0)
            return 0;

        double avg = numericCells.Average();
        double sumOfSquares = numericCells.Sum(val => Math.Pow(val - avg, 2));
        return Math.Sqrt(sumOfSquares / numericCells.Count);
    }

    /// <summary>
    /// Get comprehensive statistics summary
    /// </summary>
    public static StatisticsSummary GetSummary(IEnumerable<Cell> cells)
    {
        var cellList = cells.ToList();
        return new StatisticsSummary
        {
            Count = Count(cellList),
            CountNumeric = CountNumeric(cellList),
            Sum = Sum(cellList),
            Average = Average(cellList),
            Min = Min(cellList),
            Max = Max(cellList),
            Median = Median(cellList),
            StandardDeviation = StandardDeviation(cellList)
        };
    }
}

public class StatisticsSummary
{
    public int Count { get; set; }
    public int CountNumeric { get; set; }
    public double Sum { get; set; }
    public double Average { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
    public double Median { get; set; }
    public double StandardDeviation { get; set; }

    public override string ToString()
    {
        return $"Count: {Count}, Numeric: {CountNumeric}, Sum: {Sum:F2}, Avg: {Average:F2}, Min: {Min:F2}, Max: {Max:F2}, Median: {Median:F2}, StdDev: {StandardDeviation:F2}";
    }
}
