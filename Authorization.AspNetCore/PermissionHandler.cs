using System.Security.Claims;
using DDD.Core.Utils;
using DDD.PermissionManagement.Domain.PermissionGrants;
using DDD.PermissionManagement.Domain.Shared.PermissionGrants;
using Microsoft.AspNetCore.Authorization;

namespace DDD.Authorization.AspNetCore;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionGrantRepository _permissionGrantRepository;

    public PermissionHandler(IPermissionGrantRepository permissionGrantRepository)
    {
        _permissionGrantRepository = permissionGrantRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var userId = context.User.FindFirst("sub")?.Value;
        userId = Check.NotNullOrEmpty(userId, nameof(userId));
        var roles = context.User.FindFirst(ClaimTypes.Role)?.Value.Split(',')
                    ?? Array.Empty<string>();
        var count =
            await _permissionGrantRepository.GetCountAsync(predicate: grant =>
                grant.Name == requirement.Name
                && (
                grant.HolderName == PermissionGrantConsts.User
                   && grant.HolderKey == userId
                   || grant.HolderName == PermissionGrantConsts.Role
                   && roles.Contains(grant.HolderKey)));
        if(count > 0)
            context.Succeed(requirement);
        
    }
}