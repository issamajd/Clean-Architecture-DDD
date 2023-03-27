using DDD.Authorization.Abstractions.Permissions;

namespace DDD.Identity;

public class IdentityPermissionProvider : IPermissionProvider
{
    public void Provide(IPermissionCollection permissionCollection)
    {
        var customerGroup = permissionCollection.AddGroup("Identity.Customer", "Identity Customer");
        customerGroup.AddPermission("Identity.Customer.Read", "Identity Customer Read");
        customerGroup.AddPermission("Identity.Customer.Create", "Identity Customer Create");
        customerGroup.AddPermission("Identity.Customer.Update", "Identity Customer Update");
        
        var providerGroup = permissionCollection.AddGroup("Identity.Provider", "Identity Provider");
        providerGroup.AddPermission("Identity.Provider.Read", "Identity Provider Read");
        providerGroup.AddPermission("Identity.Provider.Create", "Identity Provider Create");
        providerGroup.AddPermission("Identity.Provider.Update", "Identity Provider Update");
        
    }
}