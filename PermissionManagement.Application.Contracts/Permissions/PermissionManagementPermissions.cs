namespace DDD.PermissionManagement.Application.Contracts.Permissions;

public static class PermissionManagementPermissions
{
    public const string GroupName = "PermissionManagement";

    public static class PermissionGrants
    {
        public const string Default = GroupName + ".PermissionGrants";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
