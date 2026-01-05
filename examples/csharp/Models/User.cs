namespace W12CSheets.Client.Models;

/// <summary>
/// Represents a user in the W12C Sheets system
/// </summary>
public class User
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public override string ToString()
    {
        return $"User[{Id}]: {Name} ({Email})";
    }
}
