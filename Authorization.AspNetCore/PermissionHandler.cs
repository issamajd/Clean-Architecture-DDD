using System.Security.Claims;
using DDD.Authorization.Abstractions.Permissions;
using DDD.Core.Utils;
using DDD.PermissionManagement.Domain.PermissionGrants;
using DDD.PermissionManagement.Domain.Shared.PermissionGrants;
using Microsoft.AspNetCore.Authorization;

namespace DDD.Authorization.AspNetCore;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly PermissionManager _permissionManager;

    public PermissionHandler(IPermissionGrantRepository permissionGrantRepository, PermissionManager permissionManager)
    {
        _permissionGrantRepository = permissionGrantRepository;
        _permissionManager = permissionManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (!context.User.Identities.Any(i => i.IsAuthenticated))
            return;
        var userId = context.User.FindFirst("sub")?.Value;
        userId = Check.NotNullOrEmpty(userId, nameof(userId));
        var roles = context.User.FindFirst(ClaimTypes.Role)?.Value.Split(',')
                    ?? Array.Empty<string>();

        var permission = _permissionManager.Get(requirement.Name);
        if (permission == null)
            return;
        var requiredPermissions = new List<string> { permission.Name };
        while (permission?.ParentName != null)
        {
            requiredPermissions.Add(permission.ParentName);
            permission = _permissionManager.Get(permission.ParentName);
        }

        var userPermissionGrants = (await _permissionGrantRepository.GetListAsync(grant =>
                grant.HolderName == PermissionGrantConsts.User
                && grant.HolderKey == userId
                || grant.HolderName == PermissionGrantConsts.Role
                && roles.Contains(grant.HolderKey))).Select(grant => grant.Name)
            .Distinct()
            .ToList();
        if (requiredPermissions.All(userPermissionGrants.Contains))
            context.Succeed(requirement);
    }
}