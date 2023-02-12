namespace DDD.Providers;

public class ProviderRepository : EfCoreRepository<Provider>, IProviderRepository
{
    public ProviderRepository(AppDbContext dbContext) : base(dbContext)
    {}
}