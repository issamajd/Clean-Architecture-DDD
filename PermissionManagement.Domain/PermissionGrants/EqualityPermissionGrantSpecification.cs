using System.Linq.Expressions;
using DDD.Core.Domain.Specifications;

namespace DDD.PermissionManagement.Domain.PermissionGrants;

public class EqualityPermissionGrantSpecification : Specification<PermissionGrant>
{
    private readonly PermissionGrant _permissionGrant;

    public EqualityPermissionGrantSpecification(PermissionGrant permissionGrant)
    {
        _permissionGrant = permissionGrant;
    }

    public override Expression<Func<PermissionGrant, bool>> ToExpression() =>
        p => p.Name == _permissionGrant.Name
             && p.HolderName == _permissionGrant.HolderName &&
             p.HolderKey == _permissionGrant.HolderKey;
}