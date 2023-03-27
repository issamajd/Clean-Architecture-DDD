using DDD.Authorization.Abstractions.Permissions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace DDD.Authorization.AspNetCore;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly PermissionManager _permissionManager;

    public PermissionPolicyProvider(PermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var permission = _permissionManager.Get(permissionName: policyName);
        if (permission == null)
            return Task.FromResult<AuthorizationPolicy?>(null);

        var policy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
        policy.AddRequirements(new PermissionRequirement(policyName));
        return Task.FromResult(policy.Build())!;
    }


    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
        Task.FromResult(new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser().Build());


    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() =>
        Task.FromResult<AuthorizationPolicy?>(null);
}