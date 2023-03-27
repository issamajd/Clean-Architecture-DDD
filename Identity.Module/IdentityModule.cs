using Autofac;
using DDD.Identity.Customers;
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
    }
}