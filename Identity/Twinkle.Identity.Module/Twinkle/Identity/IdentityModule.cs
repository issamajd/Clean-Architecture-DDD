using Autofac;
using Twinkle.Authorization.Abstractions;
using Twinkle.Identity.Customers;
using Twinkle.Identity.Permissions;
using Twinkle.Identity.Providers;

namespace Twinkle.Identity;

public class IdentityModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CustomerAppService>()
            .As<ICustomerAppService>();
        builder.RegisterType<ProviderAppService>()
            .As<IProviderAppService>();

        builder.RegisterType<EfCoreCustomerRepository>()
            .As<ICustomerRepository>();
        builder.RegisterType<EfCoreProviderRepository>()
            .As<IProviderRepository>();

        builder.RegisterType<IdentityPermissionsProvider>()
            .SingleInstance()
            .As<IPermissionProvider>()
            .AsSelf();
    }
}