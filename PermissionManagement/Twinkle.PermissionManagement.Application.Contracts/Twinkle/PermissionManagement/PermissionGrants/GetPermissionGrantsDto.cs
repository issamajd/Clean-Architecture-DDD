namespace Twinkle.PermissionManagement.PermissionGrants;

public class GetPermissionGrantsDto
{
    public ICollection<PermissionGroupDto> PermissionGroupDtos { get; set; }

    public GetPermissionGrantsDto()
    {
        PermissionGroupDtos = new List<PermissionGroupDto>();
    }
}