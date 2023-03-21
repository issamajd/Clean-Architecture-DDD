using DDD.Core.Domain;

namespace DDD.PermissionManagement.Domain.PermissionGrants;

public class HolderNotFoundException : BusinessException
{
    public HolderNotFoundException(string message) : base(message)
    {
    }
}