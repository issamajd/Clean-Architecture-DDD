using Autofac;
using DDD.Core.Domain;

namespace DDD.Core.Infrastructure.EfCore;

public static class InfrastructureEfCoreModuleBuilderExtensions
{
    public static void AddUnitOfWork(this ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .IfNotRegistered(typeof(IUnitOfWork));
    }

    public static void AddRepositories(this ContainerBuilder builder)
    {
        // Register all the Repository classes (they implement IRepository) in assembly holding the Commands
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            builder.RegisterAssemblyTypes(assembly)
                //create only services out of concrete classes todo check 
                .Where(p => p is { IsGenericType: false, IsAbstract: false })
                .AsClosedTypesOf(typeof(IRepository<>));
        }
    }
}