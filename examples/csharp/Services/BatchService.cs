using W12CSheets.Client.Models;

namespace W12CSheets.Client.Services;

/// <summary>
/// Service for batch operations on spreadsheets
/// </summary>
public class BatchService
{
    private readonly FileService _fileService;
    private readonly Utils.Logger _logger;

    public BatchService(FileService fileService)
    {
        _fileService = fileService;
        _logger = new Utils.Logger();
    }

    /// <summary>
    /// Update multiple cells in batch with progress tracking
    /// </summary>
    public async Task<int> BatchUpdateCellsAsync(string fileId, Dictionary<string, Cell> updates, int batchSize = 100)
    {
        _logger.Info($"Starting batch update of {updates.Count} cells");
        
        int totalUpdated = 0;
        var batches = updates
            .Select((kvp, index) => new { kvp, index })
            .GroupBy(x => x.index / batchSize)
            .Select(g => g.Select(x => x.kvp).ToDictionary(x => x.Key, x => x.Value))
            .ToList();

        for (int i = 0; i < batches.Count; i++)
        {
            var batch = batches[i];
            _logger.Info($"Processing batch {i + 1}/{batches.Count} ({batch.Count} cells)");
            
            try
            {
                await _fileService.UpdateCellsAsync(fileId, batch);
                totalUpdated += batch.Count;
                
                // Small delay to avoid rate limiting
                if (i < batches.Count - 1)
                {
                    await Task.Delay(100);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error updating batch {i + 1}", ex);
                throw;
            }
        }

        _logger.Info($"Batch update completed: {totalUpdated} cells updated");
        return totalUpdated;
    }

    /// <summary>
    /// Copy cells from one file to another
    /// </summary>
    public async Task CopyCellsAsync(string sourceFileId, string targetFileId, string sourceRange, string targetStartCell)
    {
        _logger.Info($"Copying cells from {sourceFileId} to {targetFileId}");

        // Get source file
        var sourceFile = await _fileService.GetFileAsync(sourceFileId);
        
        // Parse range and get cells
        var cellIds = Utils.RangeHelper.ParseRange(sourceRange);
        var cellsToCopy = new Dictionary<string, Cell>();

        foreach (var cellId in cellIds)
        {
            if (sourceFile.Cells.ContainsKey(cellId))
            {
                cellsToCopy[cellId] = sourceFile.Cells[cellId];
            }
        }

        // Calculate target positions
        var (targetCol, targetRow) = Utils.CellHelper.ParseCellId(targetStartCell);
        var (sourceStartCol, sourceStartRow) = Utils.CellHelper.ParseCellId(cellIds.First());

        var targetCells = new Dictionary<string, Cell>();
        foreach (var kvp in cellsToCopy)
        {
            var (cellCol, cellRow) = Utils.CellHelper.ParseCellId(kvp.Key);
            var offsetCol = cellCol - sourceStartCol;
            var offsetRow = cellRow - sourceStartRow;
            
            var targetCellId = Utils.CellHelper.GetCellId(targetCol + offsetCol, targetRow + offsetRow);
            targetCells[targetCellId] = kvp.Value;
        }

        // Update target file
        await _fileService.UpdateCellsAsync(targetFileId, targetCells);
        _logger.Info($"Copied {targetCells.Count} cells successfully");
    }

    /// <summary>
    /// Clear cells in a range
    /// </summary>
    public async Task ClearRangeAsync(string fileId, string range)
    {
        _logger.Info($"Clearing range {range} in file {fileId}");

        var cellIds = Utils.RangeHelper.ParseRange(range);
        var updates = new Dictionary<string, Cell>();

        foreach (var cellId in cellIds)
        {
            updates[cellId] = new Cell { Value = "" };
        }

        await _fileService.UpdateCellsAsync(fileId, updates);
        _logger.Info($"Cleared {cellIds.Count} cells");
    }

    /// <summary>
    /// Fill range with sequential numbers
    /// </summary>
    public async Task FillSequenceAsync(string fileId, string range, int startNumber = 1, int step = 1)
    {
        _logger.Info($"Filling range {range} with sequence starting at {startNumber}");

        var cellIds = Utils.RangeHelper.ParseRange(range);
        var updates = new Dictionary<string, Cell>();
        int currentNumber = startNumber;

        foreach (var cellId in cellIds)
        {
            updates[cellId] = new Cell { Value = currentNumber.ToString() };
            currentNumber += step;
        }

        await _fileService.UpdateCellsAsync(fileId, updates);
        _logger.Info($"Filled {cellIds.Count} cells with sequence");
    }

    /// <summary>
    /// Apply formula to entire range
    /// </summary>
    public async Task ApplyFormulaToRangeAsync(string fileId, string range, Func<string, string> formulaGenerator)
    {
        _logger.Info($"Applying formula to range {range}");

        var cellIds = Utils.RangeHelper.ParseRange(range);
        var updates = new Dictionary<string, Cell>();

        foreach (var cellId in cellIds)
        {
            var formula = formulaGenerator(cellId);
            updates[cellId] = new Cell { Formula = formula, Value = "" };
        }

        await _fileService.UpdateCellsAsync(fileId, updates);
        _logger.Info($"Applied formula to {cellIds.Count} cells");
    }
}
