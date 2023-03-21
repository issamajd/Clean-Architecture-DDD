using DDD.Core.Infrastructure.EfCore;
using DDD.PermissionManagement.Domain.PermissionGrants;
using Microsoft.EntityFrameworkCore;

namespace DDD.PermissionManagement.Infrastructure.EfCore;

public interface IPermissionManagementDbContext : IDbContext
{
    public DbSet<PermissionGrant> PermissionGrants { get; set; }
}