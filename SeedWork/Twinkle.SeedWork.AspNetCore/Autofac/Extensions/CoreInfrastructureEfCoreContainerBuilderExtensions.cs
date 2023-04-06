using System.Reflection;
using Twinkle.SeedWork.Application.Behaviors;
using Twinkle.SeedWork.Domain;
using Twinkle.SeedWork.Infrastructure.EfCore;
using Twinkle.SeedWork.Misc;

namespace Autofac.Extensions;

public static class CoreInfrastructureEfCoreContainerBuilderExtensions
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
}