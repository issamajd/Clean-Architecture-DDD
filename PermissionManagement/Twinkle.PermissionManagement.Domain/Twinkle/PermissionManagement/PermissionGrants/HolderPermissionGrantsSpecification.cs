using System.Linq.Expressions;
using Twinkle.SeedWork.Specifications;

namespace Twinkle.PermissionManagement.PermissionGrants;

/// <summary>
/// Specification class for get all holder permissions grants.
/// </summary>
public class HolderPermissionGrantsSpecification : Specification<PermissionGrant>
{
    private readonly string _holderKey;
    private readonly string _holderName;

    
    public HolderPermissionGrantsSpecification(string holderKey, string holderName)
    {
        _holderKey = holderKey;
        _holderName = holderName;
    }

    /// <summary>
    /// Converts the HolderPermissionGrantsSpecification object to an expression tree.
    /// </summary>
    /// <returns>An expression tree that represents the HolderPermissionGrantsSpecification object.</returns>
    public override Expression<Func<PermissionGrant, bool>> ToExpression() =>
        p => p.HolderName == _holderName &&
             p.HolderKey == _holderKey;
}