namespace Backend_API.Errors;

public class APIValidationErrorResponse : APIResponse
{
    public IEnumerable<string> Errors { get; set; }
    
    public APIValidationErrorResponse() : base(400)
    {
        
    }
}