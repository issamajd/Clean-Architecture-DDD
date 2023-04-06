namespace Twinkle.Authorization.Abstractions.Permissions;

public interface IPermissionProvider
{
    public void Provide(IPermissionCollection permissionCollection);
}