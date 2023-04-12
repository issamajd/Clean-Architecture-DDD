using Autofac;
using Twinkle.Authorization.Abstractions;
using Twinkle.PermissionManagement.PermissionGrants;
using Twinkle.PermissionManagement.Permissions;

namespace Twinkle.PermissionManagement;

public class PermissionManagementModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PermissionGrantManager>()
            .AsSelf();
        builder.RegisterType<PermissionGrantAppService>()
            .As<IPermissionGrantAppService>();
        builder.RegisterType<EfCorePermissionGrantRepository>()
            .As<IPermissionGrantRepository>();
        
        builder.RegisterType<PermissionManagementPermissionProvider>()
            .SingleInstance()
            .As<IPermissionProvider>()
            .AsSelf();
    }
}