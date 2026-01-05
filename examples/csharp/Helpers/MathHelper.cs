namespace W12CSheets.Client.Helpers;

/// <summary>
/// Math helper utilities
/// </summary>
public static class MathHelper
{
    /// <summary>
    /// Calculate percentage
    /// </summary>
    public static double Percentage(double value, double total)
    {
        if (total == 0) return 0;
        return (value / total) * 100;
    }

    /// <summary>
    /// Round to specific decimal places
    /// </summary>
    public static double RoundTo(double value, int decimalPlaces)
    {
        return Math.Round(value, decimalPlaces);
    }

    /// <summary>
    /// Clamp value between min and max
    /// </summary>
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0) return min;
        if (value.CompareTo(max) > 0) return max;
        return value;
    }

    /// <summary>
    /// Check if number is in range (inclusive)
    /// </summary>
    public static bool IsInRange<T>(T value, T min, T max) where T : IComparable<T>
    {
        return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
    }

    /// <summary>
    /// Calculate average of values
    /// </summary>
    public static double Average(params double[] values)
    {
        if (values.Length == 0) return 0;
        return values.Average();
    }

    /// <summary>
    /// Calculate median of values
    /// </summary>
    public static double Median(params double[] values)
    {
        if (values.Length == 0) return 0;

        var sorted = values.OrderBy(x => x).ToArray();
        int middle = sorted.Length / 2;

        if (sorted.Length % 2 == 0)
        {
            return (sorted[middle - 1] + sorted[middle]) / 2.0;
        }
        else
        {
            return sorted[middle];
        }
    }

    /// <summary>
    /// Calculate mode (most frequent value)
    /// </summary>
    public static double Mode(params double[] values)
    {
        if (values.Length == 0) return 0;

        return values
            .GroupBy(x => x)
            .OrderByDescending(g => g.Count())
            .First()
            .Key;
    }

    /// <summary>
    /// Calculate factorial
    /// </summary>
    public static long Factorial(int n)
    {
        if (n < 0) throw new ArgumentException("Factorial not defined for negative numbers");
        if (n == 0 || n == 1) return 1;

        long result = 1;
        for (int i = 2; i <= n; i++)
        {
            result *= i;
        }
        return result;
    }

    /// <summary>
    /// Check if number is prime
    /// </summary>
    public static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        int sqrt = (int)Math.Sqrt(number);
        for (int i = 3; i <= sqrt; i += 2)
        {
            if (number % i == 0) return false;
        }

        return true;
    }

    /// <summary>
    /// Calculate GCD (Greatest Common Divisor)
    /// </summary>
    public static int GCD(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return Math.Abs(a);
    }

    /// <summary>
    /// Calculate LCM (Least Common Multiple)
    /// </summary>
    public static int LCM(int a, int b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }

    /// <summary>
    /// Convert degrees to radians
    /// </summary>
    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    /// <summary>
    /// Convert radians to degrees
    /// </summary>
    public static double RadiansToDegrees(double radians)
    {
        return radians * 180.0 / Math.PI;
    }
}
