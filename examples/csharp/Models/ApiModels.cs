namespace W12CSheets.Client.Models;

/// <summary>
/// API request/response models
/// </summary>
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public User User { get; set; } = new();
}

public class CreateFileRequest
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateCellsRequest
{
    public Dictionary<string, Cell> Updates { get; set; } = new();
}

public class ApiError
{
    public string Error { get; set; } = string.Empty;
    public string? Details { get; set; }
}
