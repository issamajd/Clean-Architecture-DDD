using Twinkle.SeedWork;

namespace Twinkle.Identity.Providers;

public class ProviderRepository : EfCoreRepository<Provider, IIdentityDbContext>, IProviderRepository
{
    public ProviderRepository(IIdentityDbContext dbContext) : base(dbContext)
    {
    }
}