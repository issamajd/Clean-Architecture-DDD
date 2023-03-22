using Autofac;
using DDD.Core.Application.Behaviors;
using DDD.Core.Domain;
using DDD.Core.Hosting.Middlewares;
using DDD.Core.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;

namespace DDD.Core.Hosting;

public static class AppBuilderExtensions
{
    public static void AddEfCoreUnitOfWork<TDbContext>(this ContainerBuilder builder)
        where TDbContext : IDbContext
    {
        builder.Register(ctx =>
            {
                if (!ctx.IsRegistered(typeof(TDbContext)))
                    throw new InvalidOperationException($"DbContext of type {typeof(TDbContext)} was" +
                                                        $" not found, please add it using  AddDbContext function");
                return ctx.Resolve<TDbContext>();
            }).IfNotRegistered(typeof(IDbContext))
            .As<IDbContext>();

        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .IfNotRegistered(typeof(IUnitOfWork));

        builder.RegisterType<UnitOfWorkBehavior>()
            .As<IUnitOfWorkBehavior>()
            .IfNotRegistered(typeof(IUnitOfWorkBehavior));
    }

    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UnitOfWorkMiddleware>();
    }
}