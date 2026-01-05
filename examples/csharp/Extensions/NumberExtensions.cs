namespace W12CSheets.Client.Extensions;

/// <summary>
/// Extension methods for numbers
/// </summary>
public static class NumberExtensions
{
    /// <summary>
    /// Check if number is even
    /// </summary>
    public static bool IsEven(this int number) => number % 2 == 0;

    /// <summary>
    /// Check if number is odd
    /// </summary>
    public static bool IsOdd(this int number) => number % 2 != 0;

    /// <summary>
    /// Check if number is positive
    /// </summary>
    public static bool IsPositive(this int number) => number > 0;

    /// <summary>
    /// Check if number is negative
    /// </summary>
    public static bool IsNegative(this int number) => number < 0;

    /// <summary>
    /// Clamp number between min and max
    /// </summary>
    public static int Clamp(this int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    /// <summary>
    /// Convert to percentage string
    /// </summary>
    public static string ToPercentage(this double value, int decimals = 2)
    {
        return $"{value:F{decimals}}%";
    }

    /// <summary>
    /// Format as currency
    /// </summary>
    public static string ToCurrency(this decimal value, string symbol = "$")
    {
        return $"{symbol}{value:N2}";
    }

    /// <summary>
    /// Convert to ordinal string (1st, 2nd, 3rd, etc.)
    /// </summary>
    public static string ToOrdinal(this int number)
    {
        if (number <= 0) return number.ToString();

        switch (number % 100)
        {
            case 11:
            case 12:
            case 13:
                return number + "th";
        }

        switch (number % 10)
        {
            case 1: return number + "st";
            case 2: return number + "nd";
            case 3: return number + "rd";
            default: return number + "th";
        }
    }

    /// <summary>
    /// Convert to Roman numerals
    /// </summary>
    public static string ToRoman(this int number)
    {
        if (number < 1 || number > 3999)
            return number.ToString();

        var values = new[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
        var numerals = new[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

        var result = "";
        for (int i = 0; i < 13; i++)
        {
            while (number >= values[i])
            {
                number -= values[i];
                result += numerals[i];
            }
        }

        return result;
    }

    /// <summary>
    /// Check if number is between range
    /// </summary>
    public static bool IsBetween(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}
