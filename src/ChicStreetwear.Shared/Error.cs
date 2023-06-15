using ChicStreetwear.Shared.Enums;

namespace ChicStreetwear.Shared;
public class Error
{
    public string Key { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    private Error(string key, string message, ErrorType type)
    {
        Key = key;
        Message = message;
        Type = type;
    }

    public static Error NotFound(string key = "General.NotFound", string message = "A 'Not Found' error has occurred.")
    {
        return new(key, message, ErrorType.NotFound);
    }
    public static Error Validation(string key = "General.Validation", string message = "A validation error has occurred.")
    {
        return new(key, message, ErrorType.Validation);
    }

    public static Error BadRequest(string key = "General.BadRequest", string message = "A bad request error has occurred.")
    {
        return new(key, message, ErrorType.BadRequest);
    }

    public static Error New(string key, string message, ErrorType type) => new(key, message, type);
}
