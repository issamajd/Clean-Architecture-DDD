using System.Collections.Immutable;
using Twinkle.SeedWork.Utils;

namespace Twinkle.Authorization.Abstractions.Permissions;

public class PermissionGroup
{
    public string Name { get; }

    public string DisplayName { get; }

    private List<Permission> Permissions { get; }

    internal PermissionGroup(string name, string? displayName = null)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        DisplayName = displayName ?? name;
        Permissions = new List<Permission>();
    }
    /// <summary>
    /// Add <see cref="Permission"/> to the current group
    /// </summary>
    /// <param name="name">Permission Name</param>
    /// <param name="displayName">Human readable permission name</param>
    /// <returns>The newly added <see cref="Permission"/></returns>
    public Permission AddPermission(string name, string? displayName = null)
    {
        var permissionDefinition = new Permission(name, displayName: displayName);
        Permissions.Add(permissionDefinition);
        return permissionDefinition;
    }

    /// <summary>
    /// Get all permissions with their descendants 
    /// </summary>
    /// <returns>Immutable List of <see cref="Permission"/></returns>
    public IImmutableList<Permission> GetPermissionsWithDescendants() =>
        Permissions.Concat(Permissions.SelectMany(permission => permission.GetDescendants())).ToImmutableList();
    /// <summary>
    /// Get current group permissions 
    /// </summary>
    /// <returns>Immutable List of <see cref="Permission"/></returns>
    public IImmutableList<Permission> GetPermissions() => Permissions.ToImmutableList();
    
}