namespace DDD.PermissionManagement.Domain.Permissions;

public interface IPermissionManager
{
    /// <summary>
    /// Add <see cref="PermissionGroup"/> to the current context
    /// </summary>
    /// <param name="name">Group name</param>
    /// <param name="displayName">Displayed group name</param>
    /// <returns>The newly added <see cref="PermissionGroup"/></returns>
    public PermissionGroup AddGroup(string name, string? displayName = null);
}