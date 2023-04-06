using Twinkle.PermissionManagement.Domain.PermissionGrants;
using Twinkle.SeedWork.Infrastructure.EfCore;

namespace Twinkle.PermissionManagement.Infrastructure.EfCore.PermissionGrants;

public class PermissionGrantRepository : EfCoreRepository<PermissionGrant, IPermissionManagementDbContext>, IPermissionGrantRepository
{
    public PermissionGrantRepository(IPermissionManagementDbContext dbContext) : base(dbContext)
    {
    }
}