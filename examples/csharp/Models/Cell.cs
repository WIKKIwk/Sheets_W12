namespace W12CSheets.Client.Models;

/// <summary>
/// Represents a single cell in a spreadsheet
/// </summary>
public class Cell
{
    public string Value { get; set; } = string.Empty;
    public string? Formula { get; set; }
    public CellFormat? Format { get; set; }

    public bool HasFormula => !string.IsNullOrEmpty(Formula);

    public override string ToString()
    {
        if (HasFormula)
            return $"{Formula} = {Value}";
        return Value;
    }
}
