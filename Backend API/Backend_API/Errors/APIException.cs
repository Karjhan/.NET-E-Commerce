namespace Backend_API.Errors;

public class APIException : APIResponse
{
    public string Details { get; set; }
    
    public APIException(int statusCode, string message = null, string details = null) : base(statusCode, message)
    {
        Details = details;
    }
}