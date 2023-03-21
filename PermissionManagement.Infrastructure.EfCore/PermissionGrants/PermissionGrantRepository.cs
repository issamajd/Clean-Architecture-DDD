using DDD.Core.Infrastructure.EfCore;
using DDD.PermissionManagement.Domain.PermissionGrants;

namespace DDD.PermissionManagement.Infrastructure.EfCore.PermissionGrants;

public class PermissionGrantRepository : EfCoreRepository<PermissionGrant, IPermissionManagementDbContext>, IPermissionGrantRepository
{
    public PermissionGrantRepository(IPermissionManagementDbContext dbContext) : base(dbContext)
    {
    }
}