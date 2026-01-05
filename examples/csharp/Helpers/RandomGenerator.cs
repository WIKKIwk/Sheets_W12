namespace W12CSheets.Client.Helpers;

/// <summary>
/// Random data generator helper
/// </summary>
public static class RandomGenerator
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// Generate random integer in range
    /// </summary>
    public static int NextInt(int min, int max)
    {
        return _random.Next(min, max + 1);
    }

    /// <summary>
    /// Generate random double
    /// </summary>
    public static double NextDouble()
    {
        return _random.NextDouble();
    }

    /// <summary>
    /// Generate random boolean
    /// </summary>
    public static bool NextBool()
    {
        return _random.Next(2) == 1;
    }

    /// <summary>
    /// Generate random element from array
    /// </summary>
    public static T NextElement<T>(params T[] elements)
    {
        return elements[_random.Next(elements.Length)];
    }

    /// <summary>
    /// Generate random name
    /// </summary>
    public static string NextName()
    {
        string[] firstNames = { "John", "Jane", "Mike", "Sarah", "David", "Emily", "James", "Emma" };
        string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis" };
        
        return $"{NextElement(firstNames)} {NextElement(lastNames)}";
    }

    /// <summary>
    /// Generate random email
    /// </summary>
    public static string NextEmail()
    {
        string name = NextName().Replace(" ", ".").ToLower();
        string[] domains = { "gmail.com", "yahoo.com", "outlook.com", "example.com" };
        return $"{name}@{NextElement(domains)}";
    }

    /// <summary>
    /// Generate random phone number
    /// </summary>
    public static string NextPhoneNumber()
    {
        return $"+1-{NextInt(200, 999)}-{NextInt(200, 999)}-{NextInt(1000, 9999)}";
    }

    /// <summary>
    /// Generate random date in range
    /// </summary>
    public static DateTime NextDate(DateTime startDate, DateTime endDate)
    {
        var range = (endDate - startDate).Days;
        return startDate.AddDays(NextInt(0, range));
    }

    /// <summary>
    /// Generate random color
    /// </summary>
    public static string NextColor()
    {
        return $"#{_random.Next(0x1000000):X6}";
    }

    /// <summary>
    /// Generate random Lorem Ipsum text
    /// </summary>
    public static string NextLoremIpsum(int wordCount)
    {
        string[] words = { "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit", "sed", "do", "eiusmod", "tempor", "incididunt" };
        var result = new List<string>();
        
        for (int i = 0; i < wordCount; i++)
        {
            result.Add(NextElement(words));
        }
        
        return string.Join(" ", result);
    }
}
