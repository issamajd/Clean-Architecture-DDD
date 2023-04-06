namespace Twinkle.Authorization.Abstractions;

public interface IPermissionProvider
{
    public void Provide(IPermissionCollection permissionCollection);
}