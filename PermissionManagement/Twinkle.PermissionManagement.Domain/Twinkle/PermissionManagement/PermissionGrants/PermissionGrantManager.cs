using Twinkle.SeedWork;

namespace Twinkle.PermissionManagement.PermissionGrants;

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
    public async Task<PermissionGrant> CreatePermissionGrantAsync(string permissionName, string holderName,
        string holderKey)
    {
        var permissionGrant = new PermissionGrant(Guid.NewGuid(), permissionName, holderName, holderKey);

        var existedPermissionGrant = await _permissionGrantRepository.FindAsync(
            new EqualityPermissionGrantSpecification(permissionGrant));
        if (existedPermissionGrant != null)
            throw new PermissionGrantAlreadyExistsException(permissionName, holderName, holderKey);
        return permissionGrant;
        //TODO push an event to ensure consistency across aggregates (make sure holderKey is exists) 
        // if (holderName == PermissionGrantConsts.Role && await _roleManager.FindByIdAsync(holderKey) != null
        //     || holderName == PermissionGrantConsts.User && await _userManager.FindByIdAsync(holderKey) != null)
        // throw new HolderNotFoundException($"Holder name {holderName} with key ${holderKey} not found");
    }
}