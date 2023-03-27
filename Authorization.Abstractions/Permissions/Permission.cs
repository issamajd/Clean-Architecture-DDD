using DDD.Core.Utils;

namespace DDD.Authorization.Abstractions.Permissions;

public class Permission
{
    public string Name { get; set; }

    public string? ParentName { get; set; }

    public string DisplayName { get; set; }

    private List<Permission> Children { get; }

    public Permission(Permission permission) : this(permission.Name, permission.ParentName, permission.DisplayName)
    {
    }

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

    /// <summary>
    /// Get current permission children as new <see cref="Permission"/> instances
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Permission> GetChildrenRecursively()
    {
        if (Children.Count == 0)
            return Array.Empty<Permission>();

        var descendents = new List<Permission>();
        Children.ForEach(permission =>
        {
            descendents.Add(new Permission(permission));
            descendents.AddRange(permission.GetChildrenRecursively());
        });
        return descendents;
    }
}