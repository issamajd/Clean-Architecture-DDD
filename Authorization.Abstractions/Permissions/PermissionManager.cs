namespace DDD.Authorization.Abstractions.Permissions;

public class PermissionManager : IPermissionCollection
{
    protected IPermissionStore PermissionStore { get; }

    public PermissionManager(IPermissionStore permissionStore)
    {
        PermissionStore = permissionStore;
    }

    public virtual PermissionGroup AddGroup(string name, string? displayName = null)
    {
        var permissionGroup = new PermissionGroup(name, displayName);
        PermissionStore.AddGroup(permissionGroup);
        return permissionGroup;
    }

    public virtual void RemoveGroup(string name)
    {
        PermissionStore.RemoveGroup(name);
    }

    /// <summary>
    /// try to find the required permission by the name provided
    /// </summary>
    /// <param name="permissionName"></param>
    /// <returns>the required permission or null</returns>
    public virtual Permission? Get(string permissionName)
        => PermissionStore.FindPermissionByName(permissionName);

    /// <summary>
    /// Return all permissions in one list
    /// </summary>
    /// <returns>a list of permissions</returns>
    public virtual List<Permission> GetList() => PermissionStore.GetList();
}