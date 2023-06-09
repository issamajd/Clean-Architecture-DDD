namespace Twinkle.Identity.Providers;

public interface IProviderAppService
{
    Task<ProviderDto> GetAsync(Guid id);
    Task<ProviderDto> CreateAsync(RegisterProviderAccountDto registerProviderAccountDto);

    Task<ProviderDto> ChangeProviderBusinessNameAsync(ChangeProviderBusinessNameDto changeProviderBusinessNameDto);
}