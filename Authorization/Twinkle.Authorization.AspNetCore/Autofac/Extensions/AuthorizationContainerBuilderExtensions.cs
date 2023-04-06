using Autofac.Core;
using Microsoft.AspNetCore.Authorization;
using Twinkle.Authorization.Abstractions.Permissions;
using Twinkle.Authorization.AspNetCore;

namespace Autofac.Extensions;

public static class AuthorizationContainerBuilderExtensions
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
            .OnActivating(x =>
            {
                var permissionManager = x.Instance;
                foreach (var componentRegistration in x.Context.ComponentRegistry.RegistrationsFor(
                             new TypedService(typeof(IPermissionProvider))))
                {
                    if (x.Context.TryResolveService(new TypedService(componentRegistration.Activator.LimitType),
                            out var permissionProvider))
                    {
                        (permissionProvider as IPermissionProvider)!.Provide(permissionManager);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"The service {componentRegistration.Activator.LimitType} is not registered AsSelf");
                    }
                }

                permissionManager.CheckPermissionsForDuplicates();
            })
            .AutoActivate();
    }
}