using Autofac;
using DDD.Identity.Behaviors;

namespace DDD.Identity;

public class IdentityApplicationModule : Module
{
    public bool IgnoreUnitOfWorkBehavior { get; set; } = false;

    protected override void Load(ContainerBuilder builder)
    {
        if (!IgnoreUnitOfWorkBehavior)
            builder.RegisterType<UnitOfWorkBehavior>().As<IUnitOfWorkBehavior>();
        builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(p => p.IsAssignableTo(typeof(ApplicationService)))
            .AsImplementedInterfaces();
    }
}