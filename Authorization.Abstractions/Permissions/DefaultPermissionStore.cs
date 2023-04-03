using System.Collections.Immutable;

namespace DDD.Authorization.Abstractions.Permissions;

public class DefaultPermissionStore : IPermissionStore
{
    private ICollection<PermissionGroup> PermissionGroups { get; }

    public DefaultPermissionStore()
    {
        PermissionGroups = new List<PermissionGroup>();
    }

    public PermissionGroup AddGroup(PermissionGroup permissionGroup)
    {
        if (PermissionGroups.Any(group => group.Name == permissionGroup.Name))
            throw new InvalidOperationException($"Group name {permissionGroup.Name} already exist");
        PermissionGroups.Add(permissionGroup);
        return permissionGroup;
    }

    public void RemoveGroup(string name) => PermissionGroups.Remove(PermissionGroups.Single(group => group.Name == name));

    public IImmutableList<PermissionGroup> GetGroups() =>
        PermissionGroups.ToImmutableList();


    public Permission? FindPermissionByName(string permissionName) =>
        GetList().SingleOrDefault(permission => permission.Name == permissionName) ?? null;

    public IImmutableList<Permission> GetList() =>
        PermissionGroups.SelectMany(group => group.GetPermissionsWithDescendants()).ToImmutableList();
}