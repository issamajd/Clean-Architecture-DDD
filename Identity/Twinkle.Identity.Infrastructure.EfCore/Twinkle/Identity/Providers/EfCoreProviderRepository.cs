using Twinkle.SeedWork;

namespace Twinkle.Identity.Providers;

public class EfCoreProviderRepository : EfCoreRepository<Provider, IIdentityDbContext>, IProviderRepository
{
    public EfCoreProviderRepository(IIdentityDbContext dbContext) : base(dbContext)
    {
    }
}