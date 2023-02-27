namespace DDD.Identity.Providers;

public class ProviderRepository : EfCoreRepository<Provider>, IProviderRepository
{
    public ProviderRepository(AppDbContext dbContext) : base(dbContext)
    {}
}