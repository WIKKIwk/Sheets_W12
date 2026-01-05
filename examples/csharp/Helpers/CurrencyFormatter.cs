namespace W12CSheets.Client.Helpers;

/// <summary>
/// Currency formatter helper
/// </summary>
public static class CurrencyFormatter
{
    /// <summary>
    /// Format amount as currency
    /// </summary>
    public static string Format(decimal amount, string currencyCode = "USD")
    {
        var symbols = new Dictionary<string, string>
        {
            { "USD", "$" },
            { "EUR", "€" },
            { "GBP", "£" },
            { "JPY", "¥" },
            { "RUB", "₽" },
            { "UZS", "сўм" }
        };

        var symbol = symbols.GetValueOrDefault(currencyCode, "$");
        return $"{symbol}{amount:N2}";
    }

    /// <summary>
    /// Parse currency string to decimal
    /// </summary>
    public static decimal Parse(string currencyString)
    {
        var cleaned = System.Text.RegularExpressions.Regex.Replace(currencyString, @"[^\d\.\-]", "");
        return decimal.TryParse(cleaned, out var result) ? result : 0;
    }

    /// <summary>
    /// Convert currency
    /// </summary>
    public static decimal Convert(decimal amount, string fromCurrency, string toCurrency)
    {
        // Simplified conversion - in real app would use API
        var rates = new Dictionary<string, decimal>
        {
            { "USD", 1.0m },
            { "EUR", 0.85m },
            { "GBP", 0.73m },
            { "JPY", 110.0m },
            { "UZS", 11000.0m }
        };

        var fromRate = rates.GetValueOrDefault(fromCurrency, 1.0m);
        var toRate = rates.GetValueOrDefault(toCurrency, 1.0m);

        return amount / fromRate * toRate;
    }
}
