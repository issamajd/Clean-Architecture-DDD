namespace Twinkle.Authorization.Abstractions;

public interface IPermissionCollection
{
    /// <summary>
    /// Add <see cref="PermissionGroup"/> to the current context
    /// </summary>
    /// <param name="name">Group name</param>
    /// <param name="displayName">Displayed group name</param>
    /// <returns>The newly added <see cref="PermissionGroup"/></returns>
    public PermissionGroup AddGroup(string name, string? displayName = null);

    /// <summary>
    /// Remove <see cref="PermissionGroup"/> to the current context
    /// </summary>
    /// <param name="name">Group name</param>
    public void RemoveGroup(string name);
}