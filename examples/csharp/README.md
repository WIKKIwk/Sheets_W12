# W12C Sheets C# Client

Console application for interacting with W12C Sheets API using C#.

## Features

- ✅ User authentication (login)
- ✅ List all spreadsheet files
- ✅ Create new spreadsheets
- ✅ View and update cells
- ✅ CSV import/export
- ✅ Interactive console UI

## Requirements

- .NET 8.0 SDK
- W12C Sheets API running (<http://localhost:8080>)

## Building

```bash
cd examples/csharp
dotnet build
```

## Running

```bash
dotnet run
```

## Usage

### 1. Login

Enter your email and password to authenticate.

### 2. Main Menu Options

**1. List Files** - View all your spreadsheets
**2. Create File** - Create a new spreadsheet
**3. Work with File** - View and update cells in a file
**4. CSV Export/Import** - Export spreadsheet to CSV or import from CSV
**5. Exit** - Logout and exit

### 3. Working with Cells

Enter file ID to view cells:

- View first 10 cells
- Update any cell by providing cell ID (e.g., A1) and value

### 4. CSV Operations

**Export:**

- Enter file ID
- Choose output path (default: output.csv)

**Import:**

- Enter CSV file path
- Enter target file ID
- All cells will be updated from CSV

## Project Structure

```
csharp/
├── Models/
│   ├── User.cs              # User model
│   ├── SpreadsheetFile.cs   # File model
│   ├── Cell.cs              # Cell model
│   ├── CellFormat.cs        # Formatting model
│   └── ApiModels.cs         # API DTOs
├── Services/
│   ├── AuthService.cs       # Authentication
│   └── FileService.cs       # File operations
├── Utils/
│   ├── CellHelper.cs        # Cell ID utilities
│   ├── CsvHelper.cs         # CSV import/export
│   └── ConsoleHelper.cs     # Console UI helpers
├── Program.cs               # Main entry point
└── W12CSheets.Client.csproj # Project file
```

## Examples

### Login

```
Email: user@example.com
Password: ********
✓ Xush kelibsiz, John Doe!
```

### Create File

```
Fayl nomi: Budget 2026
✓ Fayl yaratildi: Budget 2026 (ID: abc123)
```

### Update Cell

```
Fayl ID: abc123
Hujayra ID: A1
Qiymat: Product Name
✓ Yangilandi!
```

### Export to CSV

```
Fayl ID: abc123
Saqlash yo'li: budget.csv
✓ CSV fayl saqlandi: budget.csv
```

## API Configuration

Default API URL: `http://localhost:8080`

To change, modify the `AuthService` and `FileService` constructors in the code.

## Dependencies

- **Newtonsoft.Json** - JSON serialization
- **System.Net.Http** - HTTP client

## License

Part of W12C Sheets project - Apache 2.0
