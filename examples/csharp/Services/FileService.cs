using System.Text;
using Newtonsoft.Json;
using W12CSheets.Client.Models;

namespace W12CSheets.Client.Services;

/// <summary>
/// Service for spreadsheet file operations
/// </summary>
public class FileService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly string _token;

    public FileService(string token, string baseUrl = "http://localhost:8080")
    {
        _token = token;
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }

    /// <summary>
    /// Get all files for current user
    /// </summary>
    public async Task<List<SpreadsheetFile>> GetFilesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/files");
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
            throw new Exception($"Failed to get files: {error?.Error}");
        }

        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
        var filesJson = result?["files"]?.ToString();
        return JsonConvert.DeserializeObject<List<SpreadsheetFile>>(filesJson!) ?? new();
    }

    /// <summary>
    /// Create a new spreadsheet file
    /// </summary>
    public async Task<SpreadsheetFile> CreateFileAsync(string name)
    {
        var request = new CreateFileRequest { Name = name };
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/files", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
            throw new Exception($"Failed to create file: {error?.Error}");
        }

        return JsonConvert.DeserializeObject<SpreadsheetFile>(responseBody)!;
    }

    /// <summary>
    /// Get specific file by ID
    /// </summary>
    public async Task<SpreadsheetFile> GetFileAsync(string fileId)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/files/{fileId}");
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
            throw new Exception($"Failed to get file: {error?.Error}");
        }

        return JsonConvert.DeserializeObject<SpreadsheetFile>(responseBody)!;
    }

    /// <summary>
    /// Update cells in a file
    /// </summary>
    public async Task UpdateCellsAsync(string fileId, Dictionary<string, Cell> updates)
    {
        var request = new UpdateCellsRequest { Updates = updates };
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync($"{_baseUrl}/api/v1/files/{fileId}/cells", content);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
            throw new Exception($"Failed to update cells: {error?.Error}");
        }
    }

    /// <summary>
    /// Delete a file
    /// </summary>
    public async Task DeleteFileAsync(string fileId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/files/{fileId}");

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
            throw new Exception($"Failed to delete file: {error?.Error}");
        }
    }
}
