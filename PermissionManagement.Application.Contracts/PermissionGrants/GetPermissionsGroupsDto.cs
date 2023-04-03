namespace DDD.PermissionManagement.Application.Contracts.PermissionGrants;

public class GetPermissionsGroupsDto
{
    public ICollection<PermissionGroupDto> PermissionGroupDtos { get; }

    public GetPermissionsGroupsDto()
    {
        PermissionGroupDtos = new List<PermissionGroupDto>();
    }
}