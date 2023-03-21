using DDD.Core.Utils;

namespace DDD.PermissionManagement.Domain.Permissions;

public class PermissionGroup
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public List<Permission> Permissions { get; }

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
}