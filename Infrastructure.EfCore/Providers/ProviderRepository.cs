namespace DDD.Providers;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _dbContext;

    public ProviderRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Provider?> GetAsync(Guid id)
    {
        var provider = await _dbContext.Providers.FindAsync(id);
        return provider;
    }

    public async Task<Provider?> CreateAsync(Provider provider)
    {
        var dbSet = _dbContext.Set<Provider>();
        var providerEntity = await dbSet.AddAsync(provider);
        await _dbContext.SaveChangesAsync();
        return providerEntity.Entity;
    }

    public async Task<Provider> ChangeBusinessNameAsync(Provider provider, string businessName)
    {
        var dbSet = _dbContext.Set<Provider>();
        dbSet.Attach(provider);
        provider.BusinessName = businessName;
        await _dbContext.SaveChangesAsync();
        return provider;
    }
}