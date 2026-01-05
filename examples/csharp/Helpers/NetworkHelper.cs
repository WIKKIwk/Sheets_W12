namespace W12CSheets.Client.Helpers;

/// <summary>
/// Network helper utilities
/// </summary>
public static class NetworkHelper
{
    /// <summary>
    /// Check if URL is reachable
    /// </summary>
    public static async Task<bool> IsUrlReachable(string url, int timeoutMs = 5000)
    {
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromMilliseconds(timeoutMs) };
            var response = await client.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Download string from URL
    /// </summary>
    public static async Task<string> DownloadString(string url)
    {
        using var client = new HttpClient();
        return await client.GetStringAsync(url);
    }

    /// <summary>
    /// Download file from URL
    /// </summary>
    public static async Task DownloadFile(string url, string destinationPath)
    {
        using var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync(url);
        await File.WriteAllBytesAsync(destinationPath, bytes);
    }

    /// <summary>
    /// Get computer's local IP address
    /// </summary>
    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        return "127.0.0.1";
    }

    /// <summary>
    /// Check if internet connection is available
    /// </summary>
    public static bool IsInternetAvailable()
    {
        try
        {
            using var client = new System.Net.NetworkInformation.Ping();
            var reply = client.Send("8.8.8.8", 3000);
            return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }
}
