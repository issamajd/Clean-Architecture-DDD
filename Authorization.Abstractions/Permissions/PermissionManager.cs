using System.Collections.Immutable;

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

    public virtual IImmutableList<PermissionGroup> GetGroups() => PermissionStore.GetGroups();

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
    public virtual IImmutableList<Permission> GetList() => PermissionStore.GetList();

    /// <summary>
    /// Check that all the permissions in the store have unique names 
    /// </summary>
    /// <exception cref="InvalidOperationException">if one or more permissions have the same name</exception>
    public void CheckPermissionsForDuplicates()
    {
        var duplicates = PermissionStore.GetList().GroupBy(permission => permission.Name)
            .Where(p => p.Count() > 1).Select(p => p.Key).ToList();
        if (duplicates.Any())
            throw new InvalidOperationException("Following permissions are duplicated, please delete them: " +
                                string.Join(",", duplicates));
    }
}