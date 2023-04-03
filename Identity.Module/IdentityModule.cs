using Autofac;
using DDD.Authorization.Abstractions.Permissions;
using DDD.Identity.Customers;
using DDD.Identity.Permissions;
using DDD.Identity.Providers;

namespace DDD.Identity.Module;

public class IdentityModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CustomerAppService>()
            .As<ICustomerAppService>();
        builder.RegisterType<ProviderAppService>()
            .As<IProviderAppService>();

        builder.RegisterType<CustomerRepository>()
            .As<ICustomerRepository>();
        builder.RegisterType<ProviderRepository>()
            .As<IProviderRepository>();
        
        builder.RegisterType<IdentityPermissionsProvider>()
            .SingleInstance()
            .As<IPermissionProvider>()
            .AsSelf();
    }
}