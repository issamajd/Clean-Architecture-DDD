using System.ComponentModel.DataAnnotations;
using DDD.Authorization.AspNetCore;
using DDD.PermissionManagement.Application.Contracts.PermissionGrants;
using DDD.PermissionManagement.Application.Contracts.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace PermissionManagement.Presentation.Controllers;

[ApiController]
[Route("permission")]
public class PermissionGrantController : ControllerBase
{
    private readonly IPermissionGrantAppService _permissionGrantAppService;

    public PermissionGrantController(IPermissionGrantAppService permissionGrantAppService)
    {
        _permissionGrantAppService = permissionGrantAppService;
    }

    [Permission(PermissionManagementPermissions.PermissionGrants.Default)]
    [HttpGet]
    public Task<GetPermissionsGroupsDto>
        GetPermissions() =>
        _permissionGrantAppService.GetPermissionsGroupsAsync();

    [Permission(PermissionManagementPermissions.PermissionGrants.Default)]
    [HttpGet]
    [Route("grants")]
    public Task<GetPermissionGrantsDto>
        GetPermissionGrants([Required] string holderName,
            [Required] string holderKey) =>
        _permissionGrantAppService.GetPermissionGrantsAsync(holderKey, holderName);

    [Permission(PermissionManagementPermissions.PermissionGrants.Edit)]
    [HttpPost]
    [Route("grants")]
    public Task UpdatePermissionGrants(UpdatePermissionGrantsDto updatePermissionGrantsDto) =>
        _permissionGrantAppService.UpdatePermissionGrants(updatePermissionGrantsDto);
}