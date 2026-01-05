namespace W12CSheets.Client.Utils;

/// <summary>
/// Console UI helper for formatting output
/// </summary>
public static class ConsoleHelper
{
    public static void WriteHeader(string text)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"═══ {text} ═══");
        Console.ResetColor();
        Console.WriteLine();
    }

    public static void WriteSuccess(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"✓ {text}");
        Console.ResetColor();
    }

    public static void WriteError(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"✗ {text}");
        Console.ResetColor();
    }

    public static void WriteInfo(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"ℹ {text}");
        Console.ResetColor();
    }

    public static string? ReadInput(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine();
    }

    public static string ReadPassword(string prompt)
    {
        Console.Write($"{prompt}: ");
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, password.Length - 1);
                Console.Write("\b \b");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }

    public static void ShowMenu(string title, params string[] options)
    {
        WriteHeader(title);
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i]}");
        }
        Console.WriteLine();
    }

    public static int GetMenuChoice(int max)
    {
        while (true)
        {
            var input = ReadInput("Tanlov");
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= max)
            {
                return choice;
            }
            WriteError("Noto'g'ri tanlov, qaytadan urinib ko'ring");
        }
    }
}
