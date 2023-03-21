using DDD.Core.Domain;
using DDD.Core.Utils;
using DDD.PermissionManagement.Domain.Shared.PermissionGrants;

namespace DDD.PermissionManagement.Domain.PermissionGrants;

public class PermissionGrant : AggregateRoot<Guid>
{
    public string Name { get; }

    public string HolderName { get; private set; } = null!;

    public string HolderKey { get; }

    public void SetHolderName(string holderName)
    {
        if (holderName != PermissionGrantConsts.Role && holderName != PermissionGrantConsts.User)
            throw new BusinessException(
                $"Holder name must be either {PermissionGrantConsts.Role} or {PermissionGrantConsts.User}");
        HolderName = holderName;
    }
    private PermissionGrant(){}

    internal PermissionGrant(string name, string holderName, string holderKey)
    {
        Name = Check.NotNullOrEmpty(name, nameof(name));
        SetHolderName(holderName);
        HolderKey = Check.NotNullOrEmpty(holderKey, nameof(holderKey));
    }
}