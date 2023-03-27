namespace DDD.Authorization.Abstractions.Permissions;

public interface IPermissionStore
{
    /// <summary>
    /// Add <see cref="PermissionGroup"/> to the current context
    /// </summary>
    /// <param name="permissionGroup">the permission group to be added</param>
    /// <returns>The newly added <see cref="PermissionGroup"/></returns>
    public PermissionGroup AddGroup(PermissionGroup permissionGroup);

    /// <summary>
    /// Remove <see cref="PermissionGroup"/> to the current context
    /// </summary>
    /// <param name="name">Group name</param>
    public void RemoveGroup(string name);
    
    /// <summary>
    /// try to find the required permission by the name provided
    /// </summary>
    /// <param name="permissionName"></param>
    /// <returns>the required permission or null</returns>
    public Permission? FindPermissionByName(string permissionName);
    
    /// <summary>
    /// Return all permissions in one list
    /// </summary>
    /// <returns>a list of permissions</returns>
    public List<Permission> GetList();
}