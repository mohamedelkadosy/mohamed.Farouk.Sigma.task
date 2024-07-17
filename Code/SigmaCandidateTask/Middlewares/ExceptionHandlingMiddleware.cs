using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.ILogger _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, Serilog.ILogger logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            // Log bad requests
            if (context.Response.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                var errorDetails = context.Response.Headers["X-Validation-Errors"].ToString();
                _logger.Warning("Bad Request: {Method} {Url} => {StatusCode}\nValidation Errors: {Errors}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    errorDetails);
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = new
        {
            message = "Internal server error. Please try again later.",
            details = ex.Message
        };

        return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
    }
}
