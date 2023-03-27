using System.Reflection;
using Autofac;
using DDD.Core.Application;

namespace DDD.Core.Hosting;

public static class CoreApplicationBuilderExtensions
{
    public static void AddApplicationServices(this ContainerBuilder builder)
    {
        var assemblies =  typeof(ApplicationService).Assembly.GetReferencingAssemblies();        
        builder.RegisterAssemblyTypes(assemblies.Select(Assembly.Load).ToArray())
            .Where(p => typeof(ApplicationService).IsAssignableFrom(p))
            .AsImplementedInterfaces();
    }
}