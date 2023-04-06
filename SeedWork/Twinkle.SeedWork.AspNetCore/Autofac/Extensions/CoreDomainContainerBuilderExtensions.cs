using System.Reflection;
using Twinkle.SeedWork.Domain;
using Twinkle.SeedWork.Misc;

namespace Autofac.Extensions;

public static class CoreDomainContainerBuilderExtensions
{
    public static void AddDomainServices(this ContainerBuilder builder)
    {
        var assemblies = typeof(IDomainService).Assembly.GetReferencingAssemblies();
        builder.RegisterAssemblyTypes(assemblies.Select(Assembly.Load).ToArray())
            .Where(p => typeof(IDomainService).IsAssignableFrom(p))
            .AsSelf();
    }
}