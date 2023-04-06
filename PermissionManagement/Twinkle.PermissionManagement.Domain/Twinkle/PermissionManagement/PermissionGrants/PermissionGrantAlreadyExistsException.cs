using Twinkle.SeedWork;

namespace Twinkle.PermissionManagement.PermissionGrants;

public class PermissionGrantAlreadyExistsException : BusinessException
{
    public PermissionGrantAlreadyExistsException(string permissionName, string holderKey, string holderName)
        : base(string.Format(PermissionGrantErrors.PermissionGrantAlreadyExist, permissionName, holderKey, holderName))
    {
    }
}