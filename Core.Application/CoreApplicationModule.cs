using Autofac;

namespace DDD.Core.Application;

public class CoreApplicationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        AddApplicationServices(builder);
    }

    private void AddApplicationServices(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(GetType().Assembly)
                .Where(p => typeof(ApplicationService).IsAssignableFrom(p))
                .AsImplementedInterfaces();
    }
}