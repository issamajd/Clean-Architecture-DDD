namespace DDD.Providers;

public interface IProviderRepository
{
    public Task ChangeBusinessNameAsync(Provider provider, string businessName);
}