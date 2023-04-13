namespace Twinkle.Authorization.Abstractions;

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

    public virtual IEnumerable<PermissionGroup> GetGroups() => PermissionStore.GetGroups();

    /// <summary>
    /// try to find the required permission by the name provided
    /// </summary>
    /// <param name="permissionName"></param>
    /// <returns>the required permission or null</returns>
    public virtual Permission? Get(string permissionName)
        => PermissionStore.FindPermissionByName(permissionName);

    /// <summary>
    /// Return all permissions
    /// </summary>
    /// <returns>Enumerable of permissions</returns>
    public virtual IEnumerable<Permission> GetPermissions() => PermissionStore.GetPermissions();

    /// <summary>
    /// Check that all the permissions in the store have unique names 
    /// </summary>
    /// <exception cref="InvalidOperationException">if one or more permissions have the same name</exception>
    public void CheckPermissionsForDuplicates()
    {
        var duplicates = PermissionStore.GetPermissions().GroupBy(permission => permission.Name)
            .Where(p => p.Count() > 1).Select(p => p.Key).ToList();
        if (duplicates.Any())
            throw new InvalidOperationException("Following permissions are duplicated, please delete them: " +
                                string.Join(",", duplicates));
    }
}