namespace Twinkle.PermissionManagement.PermissionGrants;

public class PermissionGroupDto
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public ICollection<PermissionDto> Children { get; set; }

    public PermissionGroupDto()
    {
        Children = new List<PermissionDto>();
    }
}