using System.ComponentModel.DataAnnotations;
using Twinkle.SeedWork;

namespace Twinkle.PermissionManagement.PermissionGrants;

/// <summary>
/// Represents a permission grant entity that is an aggregate root of the domain.
/// </summary>
public class PermissionGrant : AggregateRoot<Guid>

{
    [MaxLength(256)]
    public string Name { get; private set; } = null!;

    [MaxLength(2)]
    public string HolderName { get; private set; } = null!;
    [MaxLength(256)]
    public string HolderKey { get; private set; } = null!;

    /// <summary>
    /// Sets the holder name of the permission.
    /// </summary>
    /// <param name="holderName">permission holder type (Role/User)</param>
    /// <exception cref="BusinessException">Thrown when the holder name is not "Role" or "User".</exception>
    public void SetHolderName(string holderName)
    {
        if (holderName != PermissionGrantConsts.Role && holderName != PermissionGrantConsts.User)
            throw new BusinessException(
                $"Holder name must be either {PermissionGrantConsts.Role} or {PermissionGrantConsts.User}");
        HolderName = holderName;
    }
    private PermissionGrant(){}
    /// <summary>
    /// Initializes a new instance of the PermissionGrant class with the specified name, holder name, and holder key.
    /// </summary>
    /// <param name="name">The name of the permission.</param>
    /// <param name="holderName">permission holder type (Role/User)</param>
    /// <param name="holderKey">The holder key of the permission.</param>
    internal PermissionGrant(Guid id, string name, string holderName, string holderKey) : base(id)
    {
        
        Name = Check.NotNullOrEmpty(name, nameof(name));
        SetHolderName(holderName);
        HolderKey = Check.NotNullOrEmpty(holderKey, nameof(holderKey));
    }
}