using Autofac;

namespace DDD.Core.Domain;

public class CoreDomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        AddDomainServices(builder);
    }

    private void AddDomainServices(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(p => typeof(IDomainService).IsAssignableFrom(p))
                .AsSelf();
    }
}