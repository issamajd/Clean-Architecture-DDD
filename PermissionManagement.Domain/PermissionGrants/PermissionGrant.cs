using System.ComponentModel.DataAnnotations;
using DDD.Core.Domain;
using DDD.Core.Utils;
using DDD.PermissionManagement.Domain.Shared.PermissionGrants;

namespace DDD.PermissionManagement.Domain.PermissionGrants;

public class PermissionGrant : AggregateRoot<Guid>

{
    [MaxLength(256)]
    public string Name { get; private set; }

    [MaxLength(2)]
    public string HolderName { get; private set; } = null!;
    [MaxLength(256)]
    public string HolderKey { get; private set; }

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