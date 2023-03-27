using System.Reflection;
using Autofac;
using DDD.Core.Application.Behaviors;
using DDD.Core.Domain;
using DDD.Core.Hosting.Middlewares;
using DDD.Core.Infrastructure.EfCore;
using Microsoft.AspNetCore.Builder;

namespace DDD.Core.Hosting;

public static class AppBuilderExtensions
{
    public static void AddEfCoreUnitOfWork<TDbContext>(this ContainerBuilder builder)
        where TDbContext : IDbContext
    {
        builder.Register(ctx =>
            {
                if (!ctx.IsRegistered(typeof(TDbContext)))
                    throw new InvalidOperationException($"DbContext of type {typeof(TDbContext)} was" +
                                                        $" not found, please add it using  AddDbContext function");
                return ctx.Resolve<TDbContext>();
            }).IfNotRegistered(typeof(IDbContext))
            .As<IDbContext>();

        builder.RegisterType<UnitOfWork>()
            .As<IUnitOfWork>()
            .IfNotRegistered(typeof(IUnitOfWork));

        builder.RegisterType<UnitOfWorkBehavior>()
            .As<IUnitOfWorkBehavior>()
            .IfNotRegistered(typeof(IUnitOfWorkBehavior));
    }

    public static IEnumerable<AssemblyName> GetReferencingAssemblies(this Assembly assembly)
    {
        //get all assemblies that have been loaded
        var reachableAssemblies = AppDomain.CurrentDomain
            .GetAssemblies().Select(domainAssembly => domainAssembly.GetName())
            .Distinct()
            .ToList();

        int assembliesDiscoveredCount;
        do
        {
            var newAssemblies = new List<AssemblyName>();
            assembliesDiscoveredCount = reachableAssemblies.Count;
            reachableAssemblies.ForEach(reachableAssembly =>
            {
                var newReferencedAssemblies =
                    Assembly.Load(reachableAssembly).GetReferencedAssemblies()
                    .Where(referencedAssembly => reachableAssemblies.All(anyAssembly => //check that is not included before
                                                     anyAssembly.FullName != referencedAssembly.FullName) &&
                                                 newAssemblies.All(newAssembly =>
                                                     newAssembly.FullName != referencedAssembly.FullName));
                newAssemblies.AddRange(newReferencedAssemblies);
            });
            reachableAssemblies.AddRange(newAssemblies);
        } while (assembliesDiscoveredCount < reachableAssemblies.Count);

        return reachableAssemblies.Where(reachableAssembly =>
            Assembly.Load(reachableAssembly).GetReferencedAssemblies()
                .Any(referenced => referenced.FullName == assembly.FullName));
    }

    public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
    {
        return app.UseMiddleware<UnitOfWorkMiddleware>();
    }
}