using DDD.EfCore;
using DDD.Customers;

namespace DDD.Providers;

public class ProviderRepository : IProviderRepository
{
    private readonly AppDbContext _dbContext; 
    public ProviderRepository(AppDbContext dbContext) 
    {
        _dbContext = dbContext;
    }

    public async Task ChangeBusinessNameAsync(Provider provider, string businessName)
    {
        var dbSet = _dbContext.Set<Provider>();
        dbSet.Attach(provider);
        provider.BusinessName = businessName;
        await _dbContext.SaveChangesAsync();
    }
}