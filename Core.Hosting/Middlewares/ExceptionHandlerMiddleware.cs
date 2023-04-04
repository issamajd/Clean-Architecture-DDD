using System.Diagnostics;
using System.Net;
using System.Text.Json;
using DDD.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DDD.Core.Hosting.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ApiBehaviorOptions _options;

    private readonly IHostEnvironment _environment;
    // private readonly ILoggerManager _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, IHostEnvironment environment,   
        IOptions<ApiBehaviorOptions> options

        // , ILoggerManager logger
    )
    {
        // _logger = logger;
        _next = next;
        _environment = environment;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";
        var statusCode = exception switch
        {
            BusinessException => (int)HttpStatusCode.Forbidden,
            EntityNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };
        context.Response.StatusCode = statusCode;
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = context.Response.StatusCode == (int)HttpStatusCode.InternalServerError
                    && !_environment.IsDevelopment()
                    //todo use resources to get this data
                ? "Internal server error"
                : exception.Message,
            Detail = _environment.IsDevelopment() ? exception.StackTrace : "Internal server error",
        };
        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
    }
}