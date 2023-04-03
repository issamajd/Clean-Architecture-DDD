using DDD.Authorization.Abstractions.Permissions;

namespace DDD.Identity.Permissions;

public class IdentityPermissionsProvider : IPermissionProvider
{
    public void Provide(IPermissionCollection permissionCollection)
    {
        var group = permissionCollection.AddGroup(IdentityPermissions.GroupName, "Identity");
        var customerPermission = group.AddPermission(IdentityPermissions.Customers.Default, "Identity Customer Read");
        customerPermission.AddChild(IdentityPermissions.Customers.Create, "Identity Customer Create");
        customerPermission.AddChild(IdentityPermissions.Customers.Edit, "Identity Customer Edit");
        customerPermission.AddChild(IdentityPermissions.Customers.Delete, "Identity Customer Delete");

        var providerPermission = group.AddPermission(IdentityPermissions.Providers.Default, "Identity Provider Read");
        providerPermission.AddChild(IdentityPermissions.Providers.Create, "Identity Provider Create");
        providerPermission.AddChild(IdentityPermissions.Providers.Edit, "Identity Provider Edit");
        providerPermission.AddChild(IdentityPermissions.Providers.Delete, "Identity Provider Read");
    }
}