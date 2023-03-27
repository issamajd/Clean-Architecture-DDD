using System.Reflection;
using Autofac;
using DDD.Core.Domain;

namespace DDD.Core.Hosting;

public static class CoreDomainBuilderExtensions
{
    public static void AddDomainServices(this ContainerBuilder builder)
    {
        var assemblies = typeof(IDomainService).Assembly.GetReferencingAssemblies();
        builder.RegisterAssemblyTypes(assemblies.Select(Assembly.Load).ToArray())
            .Where(p => typeof(IDomainService).IsAssignableFrom(p))
            .AsSelf();
    }
}