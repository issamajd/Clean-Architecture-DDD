namespace DDD.Identity.Permissions;

public static class IdentityPermissions
{
    public const string GroupName = "Identity";

    public static class Customers
    {
        public const string Default = GroupName + ".Customers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    public static class Providers
    {
        public const string Default = GroupName + ".Providers";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
}
