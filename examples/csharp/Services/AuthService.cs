using System.Text;
using Newtonsoft.Json;
using W12CSheets.Client.Models;

namespace W12CSheets.Client.Services;

/// <summary>
/// Service for API authentication
/// </summary>
public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private string? _token;

    public AuthService(string baseUrl = "http://localhost:8080")
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
    }

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public string? Token => _token;

    /// <summary>
    /// Login with email and password
    /// </summary>
    public async Task<LoginResponse> LoginAsync(string email, string password)
    {
        var request = new LoginRequest { Email = email, Password = password };
        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/login", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
            throw new Exception($"Login failed: {error?.Error}");
        }

        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseBody);
        _token = loginResponse?.Token;

        return loginResponse!;
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    public async Task<User> GetCurrentUserAsync()
    {
        if (!IsAuthenticated)
            throw new InvalidOperationException("Not authenticated");

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");

        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/me");
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            var error = JsonConvert.DeserializeObject<ApiError>(responseBody);
            throw new Exception($"Failed to get user: {error?.Error}");
        }

        return JsonConvert.DeserializeObject<User>(responseBody)!;
    }

    /// <summary>
    /// Logout and clear token
    /// </summary>
    public void Logout()
    {
        _token = null;
        _httpClient.DefaultRequestHeaders.Clear();
    }
}
