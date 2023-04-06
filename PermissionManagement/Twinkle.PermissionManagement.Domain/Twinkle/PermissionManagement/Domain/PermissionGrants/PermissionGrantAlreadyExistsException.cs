using Twinkle.PermissionManagement.Domain.Shared.PermissionGrants;
using Twinkle.SeedWork.Domain;

namespace Twinkle.PermissionManagement.Domain.PermissionGrants;

public class PermissionGrantAlreadyExistsException : BusinessException
{
    public PermissionGrantAlreadyExistsException(string permissionName, string holderKey, string holderName)
        : base(string.Format(PermissionGrantErrors.PermissionGrantAlreadyExist, permissionName, holderKey, holderName))
    {
    }
}