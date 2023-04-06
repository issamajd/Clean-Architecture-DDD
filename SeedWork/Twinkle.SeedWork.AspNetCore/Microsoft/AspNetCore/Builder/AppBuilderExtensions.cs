using Twinkle.SeedWork.AspNetCore.ExceptionHandler;
using Twinkle.SeedWork.AspNetCore.UnitOfWork;

namespace Microsoft.AspNetCore.Builder;

public static class AppBuilderExtensions
{

    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UnitOfWorkMiddleware>();
    }
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}