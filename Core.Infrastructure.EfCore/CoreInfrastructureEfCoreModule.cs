using Autofac;
using Autofac.Core;
using DDD.Core.Domain;

namespace DDD.Core.Infrastructure.EfCore;

public class CoreInfrastructureEfCoreModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        AddEfCoreRepositories(builder);
    }

    private void AddEfCoreRepositories(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(GetType().Assembly)
            //create only services out of concrete classes  
            .Where(p => p is { IsGenericType: false })
            .OnlyIf(ctx => ctx.IsRegistered(new TypedService(typeof(IDbContext))))
            .AsClosedTypesOf(typeof(IRepository<>));
    }
}