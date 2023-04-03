using System.ComponentModel.DataAnnotations;

namespace DDD.PermissionManagement.Application.Contracts.PermissionGrants;

public class UpdatePermissionDto
{
    [Required] [MaxLength(256)] public string Name { get; set; } = null!;
    [Required] public bool IsGranted { get; set; }
}