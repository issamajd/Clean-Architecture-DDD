using Microsoft.AspNetCore.Authorization;

namespace DDD.Authorization.AspNetCore;

public class PermissionAttribute : AuthorizeAttribute
{
    private const string PolicyPrefix = "Permission";

    public PermissionAttribute(string requiredPermission) =>
        RequiredPermission = requiredPermission;

    public string? RequiredPermission
    {
        get => Policy?[PolicyPrefix.Length..];
        set => Policy = $"{PolicyPrefix}{value}";
    }
}