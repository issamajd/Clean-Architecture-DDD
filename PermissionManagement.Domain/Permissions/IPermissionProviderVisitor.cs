namespace DDD.PermissionManagement.Domain.Permissions;

public interface IPermissionProviderVisitor
{
    public void Visit(IPermissionProvider permissionProvider);
}