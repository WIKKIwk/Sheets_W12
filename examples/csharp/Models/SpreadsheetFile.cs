namespace W12CSheets.Client.Models;

/// <summary>
/// Represents a spreadsheet file
/// </summary>
public class SpreadsheetFile
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string OwnerId { get; set; } = string.Empty;
    public Dictionary<string, Cell> Cells { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return $"File[{Id}]: {Name} ({Cells.Count} cells)";
    }
}
