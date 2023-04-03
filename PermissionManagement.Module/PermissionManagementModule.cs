using Autofac;
using DDD.Authorization.Abstractions.Permissions;
using DDD.PermissionManagement.Application.Contracts.PermissionGrants;
using DDD.PermissionManagement.Application.Contracts.Permissions;
using DDD.PermissionManagement.Application.PermissionGrants;
using DDD.PermissionManagement.Domain.PermissionGrants;
using DDD.PermissionManagement.Infrastructure.EfCore.PermissionGrants;

namespace DDD.PermissionManagement.Module;

public class PermissionManagementModule : Autofac.Module
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