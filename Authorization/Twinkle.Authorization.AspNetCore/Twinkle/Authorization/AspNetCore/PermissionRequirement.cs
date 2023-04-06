using Microsoft.AspNetCore.Authorization;

namespace Twinkle.Authorization.AspNetCore;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Name { get; }

    public PermissionRequirement(string name)
    {
        Name = name;
    }
}