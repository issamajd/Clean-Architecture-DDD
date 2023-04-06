using System.ComponentModel.DataAnnotations;

namespace Twinkle.PermissionManagement.PermissionGrants;

public class UpdatePermissionDto
{
    [Required] [MaxLength(256)] public string Name { get; set; } = null!;
    [Required] public bool IsGranted { get; set; }
}