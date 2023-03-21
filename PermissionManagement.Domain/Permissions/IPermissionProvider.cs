namespace DDD.PermissionManagement.Domain.Permissions;

public interface IPermissionProvider
{
    public void Accept(IPermissionProviderVisitor visitor);
    public void Provide(IPermissionManager permissionManager);
}