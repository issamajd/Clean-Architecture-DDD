using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Twinkle.Auditing.Abstractions;

namespace Twinkle.Auditing.AspNetCore;

public class AuditingLogMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IAuditingManager _auditingManager;

    // private readonly ILoggerManager _logger;

    public AuditingLogMiddleware(RequestDelegate next,
        IAuditingManager auditingManager
    )
    {
        _next = next;
        _auditingManager = auditingManager;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method == HttpMethod.Get.Method)
        {
            await _next(httpContext);
            return;
        }

        var saveHandle = _auditingManager.BeginAuditing();
        try
        {
            _auditingManager.Current.SetRequestData(httpContext);
            await _next(httpContext);
            _auditingManager.Current.SetResponseData(httpContext);
        }
        catch (Exception ex)
        {
            _auditingManager.Current.Exceptions.Add(ex);
            throw;
        }
        finally
        {
            await saveHandle.SaveAsync();
        }
    }
}