using DDD.Core.Utils;

namespace DDD.Authorization.Abstractions.Permissions;

public class PermissionGroup
{
    public string Name { get;  }

    public string DisplayName { get; }

    private List<Permission> Permissions { get; }

    public PermissionGroup(PermissionGroup permissionGroup) : this(permissionGroup.Name, permissionGroup.DisplayName)
    {
    }

    internal PermissionGroup(string name, string? displayName = null)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        DisplayName = displayName ?? name;
        Permissions = new List<Permission>();
    }

    public Permission AddPermission(string name, string? displayName = null)
    {
        var permissionDefinition = new Permission(name, displayName: displayName);
        Permissions.Add(permissionDefinition);
        return permissionDefinition;
    }

    /// <summary>
    /// Get all group permissions as new instances of <see cref="Permission"/> 
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Permission> GetPermissions()
    {
        var result = new List<Permission>();
        Permissions.ForEach(permission =>
        {
            result.Add(new Permission(permission));
            result.AddRange(permission.GetChildrenRecursively());
        });
        return result;
    }
}