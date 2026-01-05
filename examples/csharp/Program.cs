using W12CSheets.Client.Models;
using W12CSheets.Client.Services;
using W12CSheets.Client.Utils;

namespace W12CSheets.Client;

/// <summary>
/// Main program entry point
/// </summary>
class Program
{
    private static AuthService? _authService;
    private static FileService? _fileService;
    private static User? _currentUser;

    static async Task Main(string[] args)
    {
        ConsoleHelper.WriteHeader("W12C Sheets - C# Client");
        Console.WriteLine("W12C Sheets API client dasturi");
        Console.WriteLine();

        _authService = new AuthService();

        bool running = true;
        while (running)
        {
            if (!_authService.IsAuthenticated)
            {
                await ShowLoginMenu();
            }
            else
            {
                running = await ShowMainMenu();
            }
        }

        ConsoleHelper.WriteInfo("Dastur tugatildi");
    }

    static async Task ShowLoginMenu()
    {
        ConsoleHelper.ShowMenu("Autentifikatsiya", 
            "Tizimga kirish",
            "Chiqish");

        int choice = ConsoleHelper.GetMenuChoice(2);

        if (choice == 1)
        {
            await LoginAsync();
        }
        else
        {
            Environment.Exit(0);
        }
    }

    static async Task<bool> ShowMainMenu()
    {
        ConsoleHelper.WriteInfo($"Foydalanuvchi: {_currentUser?.Name}");
        
        ConsoleHelper.ShowMenu("Asosiy menyu",
            "Fayllarni ko'rish",
            "Yangi fayl yaratish",
            "Fayl bilan ishlash",
            "CSV export/import",
            "Chiqish");

        int choice = ConsoleHelper.GetMenuChoice(5);

        try
        {
            switch (choice)
            {
                case 1:
                    await ListFilesAsync();
                    break;
                case 2:
                    await CreateFileAsync();
                    break;
                case 3:
                    await WorkWithFileAsync();
                    break;
                case 4:
                    await CsvOperationsAsync();
                    break;
                case 5:
                    _authService?.Logout();
                    ConsoleHelper.WriteSuccess("Tizimdan chiqdingiz");
                    return false;
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Xatolik: {ex.Message}");
        }

        return true;
    }

    static async Task LoginAsync()
    {
        try
        {
            var email = ConsoleHelper.ReadInput("Email");
            var password = ConsoleHelper.ReadPassword("Parol");

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ConsoleHelper.WriteError("Email va parol kiritilishi shart");
                return;
            }

            ConsoleHelper.WriteInfo("Tizimga kirilmoqda...");
            var response = await _authService!.LoginAsync(email, password);
            
            _currentUser = response.User;
            _fileService = new FileService(_authService.Token!);

            ConsoleHelper.WriteSuccess($"Xush kelibsiz, {_currentUser.Name}!");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"Login xatosi: {ex.Message}");
        }
    }

    static async Task ListFilesAsync()
    {
        ConsoleHelper.WriteInfo("Fayllar yuklanmoqda...");
        var files = await _fileService!.GetFilesAsync();

        if (files.Count == 0)
        {
            ConsoleHelper.WriteInfo("Fayllar topilmadi");
            return;
        }

        ConsoleHelper.WriteHeader($"Fayllar ({files.Count} ta)");
        foreach (var file in files)
        {
            Console.WriteLine($"- {file.Name} (ID: {file.Id})");
            Console.WriteLine($"  Yaratilgan: {file.CreatedAt:yyyy-MM-dd HH:mm}");
            Console.WriteLine();
        }
    }

    static async Task CreateFileAsync()
    {
        var name = ConsoleHelper.ReadInput("Fayl nomi");
        if (string.IsNullOrEmpty(name))
        {
            ConsoleHelper.WriteError("Fayl nomi kiritilishi shart");
            return;
        }

        ConsoleHelper.WriteInfo("Fayl yaratilmoqda...");
        var file = await _fileService!.CreateFileAsync(name);
        ConsoleHelper.WriteSuccess($"Fayl yaratildi: {file.Name} (ID: {file.Id})");
    }

    static async Task WorkWithFileAsync()
    {
        var fileId = ConsoleHelper.ReadInput("Fayl ID");
        if (string.IsNullOrEmpty(fileId))
        {
            ConsoleHelper.WriteError("Fayl ID kiritilishi shart");
            return;
        }

        ConsoleHelper.WriteInfo("Fayl yuklanmoqda...");
        var file = await _fileService!.GetFileAsync(fileId);
        
        ConsoleHelper.WriteHeader($"Fayl: {file.Name}");
        Console.WriteLine($"Hujayralar soni: {file.Cells.Count}");
        Console.WriteLine();

        if (file.Cells.Count > 0)
        {
            ConsoleHelper.WriteInfo("Birinchi 10 ta hujayra:");
            int count = 0;
            foreach (var kvp in file.Cells)
            {
                if (count++ >= 10) break;
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }

        // Update cells option
        var update = ConsoleHelper.ReadInput("Hujayrani yangilashni xohlaysizmi? (y/n)");
        if (update?.ToLower() == "y")
        {
            var cellId = ConsoleHelper.ReadInput("Hujayra ID (masalan, A1)");
            var value = ConsoleHelper.ReadInput("Qiymat");

            if (!string.IsNullOrEmpty(cellId) && !string.IsNullOrEmpty(value))
            {
                var updates = new Dictionary<string, Cell>
                {
                    { cellId, new Cell { Value = value } }
                };

                ConsoleHelper.WriteInfo("Yangilanmoqda...");
                await _fileService!.UpdateCellsAsync(fileId, updates);
                ConsoleHelper.WriteSuccess("Yangilandi!");
            }
        }
    }

    static async Task CsvOperationsAsync()
    {
        ConsoleHelper.ShowMenu("CSV operatsiyalari",
            "CSV ga export qilish",
            "CSV dan import qilish",
            "Orqaga");

        int choice = ConsoleHelper.GetMenuChoice(3);

        if (choice == 1)
        {
            await ExportToCsvAsync();
        }
        else if (choice == 2)
        {
            await ImportFromCsvAsync();
        }
    }

    static async Task ExportToCsvAsync()
    {
        var fileId = ConsoleHelper.ReadInput("Fayl ID");
        if (string.IsNullOrEmpty(fileId))
        {
            ConsoleHelper.WriteError("Fayl ID kiritilishi shart");
            return;
        }

        var file = await _fileService!.GetFileAsync(fileId);
        var csv = CsvHelper.ExportToCsv(file.Cells);

        var outputPath = ConsoleHelper.ReadInput("Saqlash yo'li (default: output.csv)");
        outputPath = string.IsNullOrEmpty(outputPath) ? "output.csv" : outputPath;

        File.WriteAllText(outputPath, csv);
        ConsoleHelper.WriteSuccess($"CSV fayl saqlandi: {outputPath}");
    }

    static async Task ImportFromCsvAsync()
    {
        var inputPath = ConsoleHelper.ReadInput("CSV fayl yo'li");
        if (string.IsNullOrEmpty(inputPath) || !File.Exists(inputPath))
        {
            ConsoleHelper.WriteError("Fayl topilmadi");
            return;
        }

        var csvContent = File.ReadAllText(inputPath);
        var cells = CsvHelper.ImportFromCsv(csvContent);

        ConsoleHelper.WriteInfo($"{cells.Count} ta hujayra topildi");

        var fileId = ConsoleHelper.ReadInput("Fayl ID (yangilash uchun)");
        if (string.IsNullOrEmpty(fileId))
        {
            ConsoleHelper.WriteError("Fayl ID kiritilishi shart");
            return;
        }

        ConsoleHelper.WriteInfo("Hujayralar yangilanmoqda...");
        await _fileService!.UpdateCellsAsync(fileId, cells);
        ConsoleHelper.WriteSuccess("Import muvaffaqiyatli!");
    }
}
