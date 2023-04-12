using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Twinkle.SeedWork.Auditing;

namespace Twinkle.Auditing.AspNetCore;

public static class AspNetCoreAuditLogDataExtensions
{
    public static void SetRequestData(this AuditLogData auditLogData, HttpContext httpContext)
    {
        var controllerName = httpContext.GetRouteData().Values["controller"];
        var actionName = httpContext.GetRouteData().Values["action"];
        // todo find a way to get userId
        var userId = httpContext.User?.FindFirst("sub")?.Value;
        auditLogData.Url = httpContext.Request.GetDisplayUrl();
        auditLogData.HttpMethod = httpContext.Request.Method;
        auditLogData.ClientIpAddress = httpContext.Connection.RemoteIpAddress.ToString();
        auditLogData.UserId = userId != null ? Guid.Parse(userId) : null;
        auditLogData.ApplicationName = AppDomain.CurrentDomain.FriendlyName;
        auditLogData.BrowserInfo = httpContext.Request.Headers[HeaderNames.UserAgent];
        auditLogData.AuditLogActionData = new AuditLogActionData
        {
            ActionName = actionName?.ToString() ?? string.Empty,
            ControllerName = controllerName?.ToString() ?? string.Empty,
        };
    }

    public static void SetResponseData(this AuditLogData auditLogData, HttpContext httpContext)
    {
        auditLogData.StatusCode = httpContext.Response.StatusCode;
    }
}

