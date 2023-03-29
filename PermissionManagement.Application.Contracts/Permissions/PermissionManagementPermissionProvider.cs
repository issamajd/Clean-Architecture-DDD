using DDD.Authorization.Abstractions.Permissions;

namespace DDD.PermissionManagement.Application.Contracts.Permissions;

public class PermissionManagementPermissionProvider : IPermissionProvider
{
    public void Provide(IPermissionCollection permissionCollection)
    {
        var group = permissionCollection.AddGroup(PermissionManagementPermissions.GroupName, "Permission Management Permission Grants");
        var customerPermission = group.AddPermission(PermissionManagementPermissions.PermissionGrants.Default, "Permission Management Permission Grants Read");
        customerPermission.AddChild(PermissionManagementPermissions.PermissionGrants.Create, "Permission Management Permission Grants Create");
        customerPermission.AddChild(PermissionManagementPermissions.PermissionGrants.Edit, "Permission Management Permission Grants Edit");
        customerPermission.AddChild(PermissionManagementPermissions.PermissionGrants.Delete, "Permission Management Permission Grants Delete");
    }
}