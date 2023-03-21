namespace DDD.PermissionManagement.Domain.Permissions;

public abstract class PermissionProvider : IPermissionProvider
{
    public virtual void Accept(IPermissionProviderVisitor visitor)
    {
        visitor.Visit(this);
    }

    public abstract void Provide(IPermissionManager permissionManager);
}