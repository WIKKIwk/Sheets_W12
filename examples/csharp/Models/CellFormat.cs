namespace W12CSheets.Client.Models;

/// <summary>
/// Represents cell formatting options
/// </summary>
public class CellFormat
{
    public bool Bold { get; set; }
    public bool Italic { get; set; }
    public bool Underline { get; set; }
    public string? BackgroundColor { get; set; }
    public string? TextColor { get; set; }
    public string? NumberFormat { get; set; }
    public TextAlignment Alignment { get; set; } = TextAlignment.Left;
}

public enum TextAlignment
{
    Left,
    Center,
    Right
}
