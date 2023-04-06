using Autofac;
using DDD.PermissionManagement.Application.Contracts.PermissionGrants;
using DDD.PermissionManagement.Application.Contracts.Permissions;
using Twinkle.Authorization.Abstractions.Permissions;
using Twinkle.PermissionManagement.Application.PermissionGrants;
using Twinkle.PermissionManagement.Domain.PermissionGrants;
using Twinkle.PermissionManagement.Infrastructure.EfCore.PermissionGrants;

namespace Twinkle.PermissionManagement;

public class PermissionManagementModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PermissionGrantManager>()
            .AsSelf();
        builder.RegisterType<PermissionGrantAppService>()
            .As<IPermissionGrantAppService>();
        builder.RegisterType<PermissionGrantRepository>()
            .As<IPermissionGrantRepository>();
        
        builder.RegisterType<PermissionManagementPermissionProvider>()
            .SingleInstance()
            .As<IPermissionProvider>()
            .AsSelf();
    }
}