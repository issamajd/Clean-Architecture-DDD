using Autofac;
using DDD.Identity.SeedWork;

namespace DDD.Identity;

public class IdentityInfrastructureEfCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
        // Register all the Repository classes (they implement IRepository) in assembly holding the Commands
        builder.RegisterAssemblyTypes(typeof(IdentityInfrastructureEfCoreModule).Assembly)
            .Where(p => p.IsGenericType == false)
            .AsClosedTypesOf(typeof(IRepository<>));
    }
}