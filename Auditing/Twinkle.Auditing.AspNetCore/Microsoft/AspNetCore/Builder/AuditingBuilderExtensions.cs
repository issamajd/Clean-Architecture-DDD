using Twinkle.Auditing.AspNetCore;

namespace Microsoft.AspNetCore.Builder;

public static class AuditingBuilderExtensions
{

    public static IApplicationBuilder UseAuditing(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuditingLogMiddleware>();
    }
}