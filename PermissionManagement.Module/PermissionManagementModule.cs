using Autofac;
using DDD.PermissionManagement.Domain.PermissionGrants;

namespace DDD.PermissionManagement.Module;

public class PermissionManagementModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<PermissionGrantManager>()
            .AsSelf();
    }
}