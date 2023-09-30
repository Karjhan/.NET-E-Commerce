using System.Net;
using System.Text.Json;
using Backend_API.Errors;

namespace Backend_API.Middlewares;

public class ExceptionMiddleware
{
    public RequestDelegate Next { get; set; }

    public ILogger<ExceptionMiddleware> Logger { get; set; }

    public IHostEnvironment Environment { get; set; }

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
    {
        Next = next;
        Logger = logger;
        Environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await Next(context);
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var response = Environment.IsDevelopment()
                ? new APIException((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace)
                : new APIException((int)HttpStatusCode.InternalServerError);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}