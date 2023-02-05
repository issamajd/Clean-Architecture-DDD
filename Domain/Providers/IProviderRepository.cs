namespace DDD.Providers;

public interface IProviderRepository
{
    public Task<Provider?> GetAsync(Guid id);
    public Task<Provider?> CreateAsync(Provider provider);
    public Task<Provider> ChangeBusinessNameAsync(Provider provider, string businessName);
}