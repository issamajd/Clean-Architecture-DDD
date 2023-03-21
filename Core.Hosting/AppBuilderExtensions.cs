using DDD.Core.Hosting.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace DDD.Core.Hosting;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UnitOfWorkMiddleware>();
    }
}