using Twinkle.SeedWork;

namespace Twinkle.PermissionManagement.PermissionGrants;

public class PermissionGrantRepository : EfCoreRepository<PermissionGrant, IPermissionManagementDbContext>, IPermissionGrantRepository
{
    public PermissionGrantRepository(IPermissionManagementDbContext dbContext) : base(dbContext)
    {
    }
}