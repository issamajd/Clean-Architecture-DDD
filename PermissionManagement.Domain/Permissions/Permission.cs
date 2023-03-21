using DDD.Core.Utils;

namespace DDD.PermissionManagement.Domain.Permissions;

public class Permission
{
    public string Name { get; set; }

    public string? ParentName { get; set; }

    public string DisplayName { get; set; }

    private List<Permission> Children { get; }

    internal Permission(string name, string? parentName = null, string? displayName = null)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        ParentName = parentName;
        DisplayName = displayName ?? name;
        Children = new List<Permission>();
    }

    public Permission AddChild(string name, string? displayName = null)
    {
        var permission = new Permission(name, displayName: displayName);
        Children.Add(permission);
        return permission;
    }
}