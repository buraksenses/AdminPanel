using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AdminPanel.Shared.Exceptions;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong: {ex.Message}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            RegistrationFailedException => StatusCodes.Status400BadRequest,
            // CategoryNotFoundException => StatusCodes.Status404NotFound,
            // CartItemNotFoundException => StatusCodes.Status404NotFound,
            // CartNotFoundException => StatusCodes.Status404NotFound,
            // CartItemAlreadyExistsException => StatusCodes.Status400BadRequest,
            // CartSaveException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var errorDetails = new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        };
        
        var errorJson = JsonConvert.SerializeObject(errorDetails);
        await context.Response.WriteAsync(errorJson);
    }

}

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}