using DDD.Core.Infrastructure.EfCore;

namespace DDD.Identity.Providers;

public class ProviderRepository : EfCoreRepository<Provider, IIdentityDbContext>, IProviderRepository
{
    public ProviderRepository(IIdentityDbContext dbContext) : base(dbContext)
    {}
}