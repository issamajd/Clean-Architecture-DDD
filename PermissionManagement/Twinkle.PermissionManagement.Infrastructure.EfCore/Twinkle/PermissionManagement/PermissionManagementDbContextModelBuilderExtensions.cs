using Microsoft.EntityFrameworkCore;
using Twinkle.PermissionManagement.PermissionGrants;

namespace Twinkle.PermissionManagement;

public static class PermissionManagementDbContextModelBuilderExtensions
{
    public static void ConfigurePermissionManagement(
        this ModelBuilder builder)
    {
        builder.SeedPermissionsData();
    }

    private static void SeedPermissionsData(this ModelBuilder builder)
    {
        // add permission management to admin role
        builder.Entity<PermissionGrant>().HasData(
            new
            {
                Id = new Guid("8b1ccc17-e356-4465-a699-bc5afcbca763"),
                Name = "PermissionManagement.PermissionGrants",
                HolderName = "R",
                HolderKey = "ADMIN"
            },
            new
            {
                Id = new Guid("2ad9ec73-974d-41a7-9fcb-0e2c12e16230"),
                Name = "PermissionManagement.PermissionGrants.Create",
                HolderName = "R",
                HolderKey = "ADMIN"
            },
            new
            {
                Id = new Guid("cb535466-ea60-4f9b-b222-81a5ef93adab"),
                Name = "PermissionManagement.PermissionGrants.Edit",
                HolderName = "R",
                HolderKey = "ADMIN"
            },new
            {
                Id = new Guid("1f23c132-1633-40e3-8606-a1c50ffc2db7"),
                Name = "PermissionManagement.PermissionGrants.Delete",
                HolderName = "R",
                HolderKey = "ADMIN"
            }
        );
    }
}
