namespace W12CSheets.Client.Models;

/// <summary>
/// Exception models for error handling
/// </summary>
public class W12CException : Exception
{
    public string? ErrorCode { get; set; }
    public int? StatusCode { get; set; }

    public W12CException(string message) : base(message) { }
    
    public W12CException(string message, Exception innerException) : base(message, innerException) { }
    
    public W12CException(string message, string errorCode, int statusCode) : base(message)
    {
        ErrorCode = errorCode;
        StatusCode = statusCode;
    }
}

public class AuthenticationException : W12CException
{
    public AuthenticationException(string message) : base(message, "AUTH_ERROR", 401) { }
}

public class ValidationException : W12CException
{
    public Dictionary<string, string> ValidationErrors { get; set; } = new();

    public ValidationException(string message) : base(message, "VALIDATION_ERROR", 400) { }
    
    public ValidationException(string message, Dictionary<string, string> errors) : base(message, "VALIDATION_ERROR", 400)
    {
        ValidationErrors = errors;
    }
}

public class NotFoundException : W12CException
{
    public NotFoundException(string message) : base(message, "NOT_FOUND", 404) { }
}

public class RateLimitException : W12CException
{
    public int RetryAfterSeconds { get; set; }

    public RateLimitException(string message, int retryAfter = 60) : base(message, "RATE_LIMIT", 429)
    {
        RetryAfterSeconds = retryAfter;
    }
}

public class ServerException : W12CException
{
    public ServerException(string message) : base(message, "SERVER_ERROR", 500) { }
    
    public ServerException(string message, Exception innerException) : base(message, innerException)
    {
        ErrorCode = "SERVER_ERROR";
        StatusCode = 500;
    }
}
