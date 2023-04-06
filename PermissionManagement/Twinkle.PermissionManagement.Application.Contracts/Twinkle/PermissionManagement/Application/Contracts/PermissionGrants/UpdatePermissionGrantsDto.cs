using System.ComponentModel.DataAnnotations;

namespace Twinkle.PermissionManagement.Application.Contracts.PermissionGrants;

public class UpdatePermissionGrantsDto
{
    [Required] [MaxLength(256)] public string HolderKey { get; set; } = null!;
    [Required] public string HolderName { get; set; } = null!;
    [Required] public ICollection<UpdatePermissionDto> PermissionDtos { get; set; }

    public UpdatePermissionGrantsDto()
    {
        PermissionDtos = new List<UpdatePermissionDto>();
    }
}