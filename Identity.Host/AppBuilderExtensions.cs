using DDD.Identity.Middlewares;

namespace DDD.Identity;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UnitOfWorkMiddleware>();
    }
}