using Mapster;
using Microsoft.Extensions.Logging;
using Twinkle.Authorization.Abstractions.Permissions;
using Twinkle.PermissionManagement.Application.Contracts.PermissionGrants;
using Twinkle.PermissionManagement.Domain.PermissionGrants;

namespace Twinkle.PermissionManagement.Application.PermissionGrants;

public class PermissionGrantAppService : IPermissionGrantAppService
{
    private readonly PermissionManager _permissionManager;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly PermissionGrantManager _permissionGrantManager;
    private readonly ILogger<PermissionGrantAppService> _logger;

    public PermissionGrantAppService(PermissionManager permissionManager,
        IPermissionGrantRepository permissionGrantRepository,
        PermissionGrantManager permissionGrantManager,
        ILogger<PermissionGrantAppService> logger)
    {
        _permissionManager = permissionManager;
        _permissionGrantRepository = permissionGrantRepository;
        _permissionGrantManager = permissionGrantManager;
        _logger = logger;
    }


    public async Task<GetPermissionGrantsDto> GetPermissionGrantsAsync(string holderKey, string holderName)
    {
        _logger.LogInformation("Get all permissions grants for {holderKey}/{holderName}", holderKey, holderName);
        // Get a list of permission grants for the specified permission holder.
        var permissionsGrants = await _permissionGrantRepository.GetListAsync(
            new HolderPermissionGrantsSpecification(holderKey, holderName));

        // Get a list of permission groups with their respective permissions.
        var getPermissionsGroupsDto = await GetPermissionsGroupsAsync();
        // Iterate through each permission group and its corresponding permissions, and determine whether or not each permission is granted to the specified permission holder.
        foreach (var group in getPermissionsGroupsDto.PermissionGroupDtos)
        {
            var permissionDtos = group.Children.ToList();
            do
            {
                var permissionDto = permissionDtos.First();
                permissionDtos.Remove(permissionDto);

                permissionDto.IsGranted =
                    permissionsGrants.Any(permissionGrant => permissionGrant.Name == permissionDto.Name);

                permissionDtos.AddRange(permissionDto.Children);
            } while (permissionDtos.Any());
        }

        // Create a new GetPermissionGrantsDto object and set its PermissionGroupDtos property
        // to the list of permission groups with their respective permissions and whether or
        // not each permission is granted to the specified permission holder.
        return new GetPermissionGrantsDto
        {
            PermissionGroupDtos = getPermissionsGroupsDto.PermissionGroupDtos
        };
    }

    public Task<GetPermissionsGroupsDto> GetPermissionsGroupsAsync()
    {
        _logger.LogInformation("Get all permissions groups with permissions descendants");
        // Get a list of permission groups.
        var groups = _permissionManager.GetGroups();

        var result = new GetPermissionsGroupsDto();

        // Iterate through each permission group and create a PermissionGroupDto object for it.
        foreach (var permissionGroup in groups)
        {
            var permissionGroupDto = new PermissionGroupDto
            {
                Name = permissionGroup.Name,
                DisplayName = permissionGroup.DisplayName,
                // Set the Children property of the PermissionGroupDto object to the list of permissions of the permission group, adapted to a list of PermissionDto objects.
                Children = permissionGroup.GetPermissions()
                    .Adapt<ICollection<PermissionDto>>()
            };

            result.PermissionGroupDtos.Add(permissionGroupDto);
        }

        return Task.FromResult(result);
    }

    public async Task UpdatePermissionGrants(UpdatePermissionGrantsDto updatePermissionGrantsDto)
    {
        _logger.LogInformation("set new permissions grants list for ${holderKey}/${holderName}",
            updatePermissionGrantsDto.HolderKey, updatePermissionGrantsDto.HolderName);
        // Get a list of permission grants for the specified permission holder.
        var permissionsGrants = await _permissionGrantRepository.GetListAsync(
            new HolderPermissionGrantsSpecification(updatePermissionGrantsDto.HolderKey,
                updatePermissionGrantsDto.HolderName));

        var deletedPermissionGrants = permissionsGrants.Where(grant =>
            updatePermissionGrantsDto.PermissionDtos.All(permission =>
                permission.Name != grant.Name || permission.IsGranted == false)).ToList();
        await _permissionGrantRepository.DeleteAsync(grant => deletedPermissionGrants.Contains(grant));

        var createdPermissionGrants = updatePermissionGrantsDto.PermissionDtos
            .Where(permissionDto => permissionDto.IsGranted).ToList();

        foreach (var createdPermissionGrant in createdPermissionGrants)
        {
            var permissionGrant = await _permissionGrantManager.CreatePermissionGrantAsync(createdPermissionGrant.Name,
                updatePermissionGrantsDto.HolderName, updatePermissionGrantsDto.HolderKey);
            await _permissionGrantRepository.AddAsync(permissionGrant);
        }
    }
}