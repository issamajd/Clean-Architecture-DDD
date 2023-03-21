using DDD.Core.Domain;

namespace DDD.PermissionManagement.Domain.PermissionGrants;

public class PermissionGrantManager : IDomainService
{
    private readonly IPermissionGrantRepository _permissionGrantRepository;

    public PermissionGrantManager(IPermissionGrantRepository permissionGrantRepository)
    {
        _permissionGrantRepository = permissionGrantRepository;
    }

    /// <summary>
    /// Grant a new permission to a holder 
    /// </summary>
    /// <param name="permissionName">the permission to be granted to the key holder</param>
    /// <param name="holderName">permission holder type (Role/User)</param>
    /// <param name="holderKey">the permission holder</param>
    /// <returns>the new permission grant created</returns>
    /// <exception cref="HolderNotFoundException"></exception>
    public async Task<PermissionGrant> CreatePermissionGrant(string permissionName, string holderName, string holderKey)
    {
        var permissionGrant = new PermissionGrant(permissionName, holderName, holderKey);

        var existedPermissionGrant = await _permissionGrantRepository.FindAsync(
            new EqualityPermissionGrantSpecification(permissionGrant));

        if (existedPermissionGrant != null)
            return existedPermissionGrant;

        return await _permissionGrantRepository.AddAsync(permissionGrant);
        //TODO push an event to ensure consistency across aggregates (make sure holderKey is exists) 
        // if (holderName == PermissionGrantConsts.Role && await _roleManager.FindByIdAsync(holderKey) != null
        //     || holderName == PermissionGrantConsts.User && await _userManager.FindByIdAsync(holderKey) != null)
        // throw new HolderNotFoundException($"Holder name {holderName} with key ${holderKey} not found");
    }

    public Task RevokePermission(string permissionName, string holderName, string holderKey)
        => _permissionGrantRepository.DeleteAsync(
            new EqualityPermissionGrantSpecification(
                new PermissionGrant(permissionName, holderName, holderKey)));
}