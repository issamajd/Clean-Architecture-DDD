using System.Reflection;
using Autofac;
using DDD.Authorization.Abstractions.Permissions;
using DDD.Authorization.AspNetCore;
using DDD.Core.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace DDD.Authorization.Hosting;

public static class AuthorizationExtensionBuilder
{
    public static void AddAuthorizationCoreServices(this ContainerBuilder builder)
    {
        builder.RegisterType<DefaultPermissionStore>()
            .As<IPermissionStore>()
            .SingleInstance()
            .IfNotRegistered(typeof(IPermissionStore));
        
        builder.RegisterType<PermissionHandler>()
            .As<IAuthorizationHandler>()
            .SingleInstance();
        
        builder.RegisterType<PermissionPolicyProvider>()
            .As<IAuthorizationPolicyProvider>()
            .SingleInstance();

        builder.RegisterType<PermissionManager>()
            .As<IPermissionCollection>()
            .AsSelf()
            .SingleInstance()
            .OnActivating(x => Console.WriteLine(x.Instance))
            .AutoActivate();
    }

    
    public static void AddPermissionProviders(this ContainerBuilder builder)
    {
        var assemblies = typeof(IPermissionProvider).Assembly.GetReferencingAssemblies();
        builder.RegisterAssemblyTypes(assemblies.Select(Assembly.Load).ToArray())
            .Where(p => typeof(IPermissionProvider).IsAssignableFrom(p))
            .As<IPermissionProvider>()
            .OnActivating(x => 
                (x.Instance as IPermissionProvider)!.Provide(x.Context.Resolve<IPermissionCollection>()))
            .SingleInstance()
            .AutoActivate();
    }
}