namespace DDD.PermissionManagement.Domain.Permissions;

public class PermissionManager : IPermissionManager, IPermissionProviderVisitor
{
    public List<PermissionGroup> PermissionGroups { get; }

    public PermissionManager()
    {
        PermissionGroups = new List<PermissionGroup>();
    }

    public PermissionGroup AddGroup(string name, string? displayName = null)
    {
        var permissionGroup = new PermissionGroup(name, displayName);
        PermissionGroups.Add(permissionGroup);
        return permissionGroup;
    }

    public void Visit(IPermissionProvider permissionProvider)
    {
        permissionProvider.Provide(this);
    }
}