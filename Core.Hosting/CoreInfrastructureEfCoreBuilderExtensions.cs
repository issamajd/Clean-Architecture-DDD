using System.Reflection;
using Autofac;
using DDD.Core.Domain;
using DDD.Core.Infrastructure.EfCore;

namespace DDD.Core.Hosting;

public static class CoreInfrastructureEfCoreBuilderExtensions
{
    //TODO use one dbcontext to add
    public static void AutoAddDbContextServices<TDbContext>(this ContainerBuilder builder)
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

    }

    public static void AutoAddEfCoreRepositories<TDbContext>(this ContainerBuilder builder)
        where TDbContext : IDbContext
    {
        builder.AutoAddDbContextServices<TDbContext>();
        var assemblies = typeof(IRepository<>).Assembly.GetReferencingAssemblies();
        builder.RegisterAssemblyTypes(assemblies.Select(Assembly.Load).ToArray())
            //create only services out of concrete classes  
            .Where(p => p is { IsGenericType: false })
            .AsClosedTypesOf(typeof(IRepository<>));
    }
}