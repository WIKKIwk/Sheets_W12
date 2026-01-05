namespace W12CSheets.Client.Constants;

/// <summary>
/// Application-wide constants
/// </summary>
public static class AppConstants
{
    // API Endpoints
    public const string API_VERSION = "v1";
    public const string DEFAULT_API_URL = "http://localhost:8080";
    public const string API_PREFIX = "/api/v1";
    
    // Endpoints
    public const string ENDPOINT_LOGIN = "/api/v1/login";
    public const string ENDPOINT_REGISTER = "/api/v1/register";
    public const string ENDPOINT_ME = "/api/v1/me";
    public const string ENDPOINT_FILES = "/api/v1/files";
    public const string ENDPOINT_HEALTH = "/health";
    
    // Cell Constraints
    public const int MAX_CELL_VALUE_LENGTH = 32767;
    public const int MAX_FORMULA_LENGTH = 8192;
    public const int MAX_ROWS = 1000000;
    public const int MAX_COLS = 16384; // A to XFD
    
    // File Constraints
    public const int MAX_FILE_NAME_LENGTH = 255;
    public const int MAX_FILES_PER_USER = 1000;
    public const long MAX_FILE_SIZE_BYTES = 10 * 1024 * 1024; // 10 MB
    
    // Performance
    public const int DEFAULT_BATCH_SIZE = 100;
    public const int DEFAULT_PAGE_SIZE = 20;
    public const int DEFAULT_TIMEOUT_MS = 30000;
    public const int MAX_RETRY_ATTEMPTS = 3;
    
    // Security
    public const int MIN_PASSWORD_LENGTH = 8;
    public const int MAX_PASSWORD_LENGTH = 128;
    public const int TOKEN_EXPIRY_HOURS = 24;
    
    // Formatting
    public const string DATE_FORMAT = "yyyy-MM-dd";
    public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
    public const string TIME_FORMAT = "HH:mm:ss";
    
    // File Types
    public const string FILE_TYPE_CSV = ".csv";
    public const string FILE_TYPE_JSON = ".json";
    public const string FILE_TYPE_XLSX = ".xlsx";
    
    // Regex Patterns
    public const string CELL_ID_PATTERN = @"^([A-Z]+)(\d+)$";
    public const string EMAIL_PATTERN = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string URL_PATTERN = @"^https?://";
}

/// <summary>
/// Error messages
/// </summary>
public static class ErrorMessages
{
    public const string INVALID_EMAIL = "Invalid email format";
    public const string INVALID_PASSWORD = "Password must be at least 8 characters";
    public const string INVALID_CELL_ID = "Invalid cell ID format";
    public const string INVALID_RANGE = "Invalid range format";
    public const string NOT_AUTHENTICATED = "User is not authenticated";
    public const string FILE_NOT_FOUND = "File not found";
    public const string UNAUTHORIZED = "Unauthorized access";
    public const string SERVER_ERROR = "Server error occurred";
    public const string NETWORK_ERROR = "Network error occurred";
    public const string TIMEOUT_ERROR = "Request timeout";
}

/// <summary>
/// Success messages
/// </summary>
public static class SuccessMessages
{
    public const string LOGIN_SUCCESS = "Login successful";
    public const string LOGOUT_SUCCESS = "Logout successful";
    public const string FILE_CREATED = "File created successfully";
    public const string FILE_UPDATED = "File updated successfully";
    public const string FILE_DELETED = "File deleted successfully";
    public const string CELLS_UPDATED = "Cells updated successfully";
}
