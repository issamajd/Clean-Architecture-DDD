namespace Twinkle.PermissionManagement.Application.Contracts.PermissionGrants;

public interface IPermissionGrantAppService
{
    /// <summary>
    /// Gets a list of permission grants for a specific permission holder asynchronously.
    /// </summary>
    /// <param name="holderKey">The key of the permission holder.</param>
    /// <param name="holderName">permission holder type (Role/User)</param>
    /// <returns>A task containing a <see cref="GetPermissionGrantsDto"/> object that contains a list of permission groups with their respective permissions,
    /// and whether or not each permission is granted to the specified permission holder.</returns>
    public Task<GetPermissionGrantsDto> GetPermissionGrantsAsync(string holderKey, string holderName);

    /// <summary>
    /// Gets a list of permission groups with their respective permissions asynchronously.
    /// </summary>
    /// <returns>A task containing a <see cref="GetPermissionsGroupsDto"/> object that contains a list of permission groups with their respective permissions.</returns>
    public Task<GetPermissionsGroupsDto> GetPermissionsGroupsAsync();

    /// <summary>
    /// Updates the permission grants for a specified permission holder based on the given list of permissions.
    /// </summary>
    /// <param name="updatePermissionGrantsDto">The DTO containing the permission holder information and the list of permissions to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task UpdatePermissionGrants(UpdatePermissionGrantsDto updatePermissionGrantsDto);
}