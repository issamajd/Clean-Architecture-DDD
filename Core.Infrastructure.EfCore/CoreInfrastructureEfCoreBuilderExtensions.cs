using System.Reflection;
using Autofac;
using DDD.Core.Domain;

namespace DDD.Core.Infrastructure.EfCore;

public static class CoreInfrastructureEfCoreBuilderExtensions
{
    public static void AddEfCoreRepositories<TDbContext>(this ContainerBuilder builder, Assembly assembly)
        where TDbContext : IDbContext
    {
        builder.AddEfCoreRepositories<TDbContext>(new[] { assembly });
    }

    public static void AddEfCoreRepositories<TDbContext>(this ContainerBuilder builder, Assembly[] assemblies)
        where TDbContext : IDbContext
    {
        var allInterfaces = typeof(TDbContext).GetInterfaces();
        var directInterfaces = allInterfaces.Except
            (allInterfaces.SelectMany(t => t.GetInterfaces()));

        builder.Register(ctx =>
            {
                if (!ctx.IsRegistered(typeof(TDbContext)))
                    throw new InvalidOperationException($"DbContext of type {typeof(TDbContext)} was" +
                                                        $" not found, please add it using  AddDbContext function");
                return ctx.Resolve<TDbContext>();
            })
            //add all direct interfaces supposedly defined in efcore modules
            //with IDbContext interface in case the developer didn't add an interface in one of the modules
            .As(directInterfaces.Append(typeof(IDbContext)).ToArray());

        builder.RegisterAssemblyTypes(assemblies)
            //create only services out of concrete classes  
            .Where(p => p is { IsGenericType: false })
            .AsClosedTypesOf(typeof(IRepository<>));
    }
}