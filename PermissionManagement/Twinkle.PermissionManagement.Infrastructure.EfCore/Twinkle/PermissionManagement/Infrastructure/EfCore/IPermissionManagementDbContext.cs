using Microsoft.EntityFrameworkCore;
using Twinkle.PermissionManagement.Domain.PermissionGrants;
using Twinkle.SeedWork.Infrastructure.EfCore;

namespace Twinkle.PermissionManagement.Infrastructure.EfCore;

public interface IPermissionManagementDbContext : IDbContext
{
    public DbSet<PermissionGrant> PermissionGrants { get; set; }
}