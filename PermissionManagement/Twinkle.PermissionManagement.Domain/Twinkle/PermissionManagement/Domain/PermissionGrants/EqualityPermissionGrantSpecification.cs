using System.Linq.Expressions;
using Twinkle.SeedWork.Specifications;

namespace Twinkle.PermissionManagement.Domain.PermissionGrants;

/// <summary>
/// Specification class for determining equality between a PermissionGrant object and another PermissionGrant object.
/// </summary>
public class EqualityPermissionGrantSpecification : Specification<PermissionGrant>
{
    private readonly PermissionGrant _permissionGrant;

    /// <summary>
    /// Initializes a new instance of the EqualityPermissionGrantSpecification class with a specified PermissionGrant object to compare against.
    /// </summary>
    /// <param name="permissionGrant">The PermissionGrant object to compare against.</param>
    public EqualityPermissionGrantSpecification(PermissionGrant permissionGrant)
    {
        _permissionGrant = permissionGrant;
    }

    /// <summary>
    /// Converts the EqualityPermissionGrantSpecification object to an expression tree.
    /// </summary>
    /// <returns>An expression tree that represents the EqualityPermissionGrantSpecification object.</returns>
    public override Expression<Func<PermissionGrant, bool>> ToExpression() =>
        p => p.Name == _permissionGrant.Name
             && p.HolderName == _permissionGrant.HolderName &&
             p.HolderKey == _permissionGrant.HolderKey;
}