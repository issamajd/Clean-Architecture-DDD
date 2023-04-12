using Twinkle.SeedWork;

namespace Twinkle.PermissionManagement.PermissionGrants;

public class EfCorePermissionGrantRepository : EfCoreRepository<PermissionGrant, IPermissionManagementDbContext>, IPermissionGrantRepository
{
    public EfCorePermissionGrantRepository(IPermissionManagementDbContext dbContext) : base(dbContext)
    {
    }
}