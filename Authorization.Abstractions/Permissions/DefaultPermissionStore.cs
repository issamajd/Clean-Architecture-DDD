namespace DDD.Authorization.Abstractions.Permissions;

public class DefaultPermissionStore : IPermissionStore
{
    private List<PermissionGroup> _permissionGroups { get; }

    private List<Permission> _permissions;

    public DefaultPermissionStore()
    {
        _permissionGroups = new List<PermissionGroup>();
        _permissions = new List<Permission>();
    }

    public PermissionGroup AddGroup(PermissionGroup permissionGroup)
    {
        _permissionGroups.Add(permissionGroup);
        return permissionGroup;
    }

    public void RemoveGroup(string name) => _permissions.RemoveAll(group => group.Name == name);


    public Permission? FindPermissionByName(string permissionName)
    {
        Permission? result;
        if (_permissions.Count > 0)
            result = _permissions.SingleOrDefault(permission => permission.Name == permissionName);
        else
            result = _permissionGroups.Count > 0
                ? GetList().SingleOrDefault(permission => permission.Name == permissionName)
                : null;
        return result !=null ? new Permission(result) : null;
    }

    public List<Permission> GetList()
    {
        var result = new List<Permission>();

        if (_permissions.Count == 0)
            _permissionGroups.ForEach(group =>
            {
                result.AddRange(group.GetPermissions());
                _permissions.AddRange(group.GetPermissions());
            });

        else
            _permissions.ForEach(permission => result.Add(new Permission(permission)));

        return result;
    }
}