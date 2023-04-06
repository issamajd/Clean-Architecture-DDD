using System.Reflection;
using Twinkle.SeedWork;
using Twinkle.SeedWork.Misc;

namespace Autofac.Extensions;

public static class CoreApplicationContainerBuilderExtensions
{
    public static void AutoAddApplicationServices(this ContainerBuilder builder)
    {
        var assemblies = typeof(ApplicationService).Assembly.GetReferencingAssemblies();
        builder.RegisterAssemblyTypes(assemblies.Select(Assembly.Load).ToArray())
            .Where(p => typeof(ApplicationService).IsAssignableFrom(p))
            .AsImplementedInterfaces();
    }
}