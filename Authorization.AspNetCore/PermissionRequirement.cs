using Microsoft.AspNetCore.Authorization;

namespace DDD.Authorization.AspNetCore;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Name { get; }

    public PermissionRequirement(string name)
    {
        Name = name;
    }
}