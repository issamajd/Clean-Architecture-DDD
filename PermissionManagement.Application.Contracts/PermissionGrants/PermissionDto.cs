namespace DDD.PermissionManagement.Application.Contracts.PermissionGrants;

public class PermissionDto 
{
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public bool IsGranted { get; set; }
    public ICollection<PermissionDto> Children { get; set; }

    public PermissionDto()
    {
        Children = new List<PermissionDto>();
    }
}