using Microsoft.EntityFrameworkCore;
using Twinkle.PermissionManagement.PermissionGrants;
using Twinkle.SeedWork;

namespace Twinkle.PermissionManagement;

public interface IPermissionManagementDbContext : IDbContext
{
    public DbSet<PermissionGrant> PermissionGrants { get; set; }
}