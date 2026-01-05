namespace W12CSheets.Client.Models;

/// <summary>
/// Configuration settings for the application
/// </summary>
public class AppConfig
{
    public string ApiBaseUrl { get; set; } = "http://localhost:8080";
    public int RequestTimeout { get; set; } = 30000; // milliseconds
    public bool EnableLogging { get; set; } = true;
    public string LogLevel { get; set; } = "Info";

    public static AppConfig Default => new AppConfig();

    public static AppConfig FromEnvironment()
    {
        return new AppConfig
        {
            ApiBaseUrl = Environment.GetEnvironmentVariable("W12C_API_URL") ?? "http://localhost:8080",
            RequestTimeout = int.Parse(Environment.GetEnvironmentVariable("W12C_TIMEOUT") ?? "30000"),
            EnableLogging = bool.Parse(Environment.GetEnvironmentVariable("W12C_LOGGING") ?? "true"),
            LogLevel = Environment.GetEnvironmentVariable("W12C_LOG_LEVEL") ?? "Info"
        };
    }
}
