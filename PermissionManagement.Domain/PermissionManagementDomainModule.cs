using Autofac;
using DDD.PermissionManagement.Domain.Permissions;

namespace DDD.PermissionManagement.Domain;

public class PermissionManagementDomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //TODO use these as addpermissions extension method in permission module
        // Permission Definition Providers registration
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) //Get all assemblies in the current app
        {
            builder.RegisterAssemblyTypes(assembly)
                .Where(p => typeof(IPermissionProvider).IsAssignableFrom(p))
                .AsSelf()
                .SingleInstance(); //Create a single instance for current container lifetime
        }

        // Register Permission Definition Manager after resolving all definition providers
        builder.Register(c =>
            {
                var visitor = new PermissionManager();
                IList<Type> definitionProvidersTypes = (from registration in c.ComponentRegistry.Registrations
                    where typeof(IPermissionProvider).IsAssignableFrom(registration.Activator.LimitType)
                    select registration.Activator.LimitType).ToList();

                definitionProvidersTypes
                    .Select(c.Resolve)
                    .Cast<IPermissionProvider>()
                    .ToList()
                    .ForEach(provider => provider.Accept(visitor)); //visit all providers

                return visitor;
            })
            .IfNotRegistered(typeof(IPermissionManager))
            .SingleInstance()
            .As<PermissionManager>();
    }
}