using DDD.AppIdentity;
using DDD.Customers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DDD.Providers;

public class ProviderAppService : IProviderAppService
{
    private readonly IProviderRepository _providerRepository;
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProviderAppService(IProviderRepository providerRepository, UserManager<AppIdentityUser> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _providerRepository = providerRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProviderDto> GetAsync(Guid id)
    {
        var provider = await _providerRepository.GetAsync(id);
        return new ProviderDto
        {
            BusinessName = provider.BusinessName, Id = provider.Id, UserId = provider.UserId
        };
    }

    public async Task<ProviderDto> CreateAsync(RegisterProviderAccountDto registerProviderAccountDto)
    {
        var user = new AppIdentityUser()
        {
            UserName = registerProviderAccountDto.Username,
        };
        var result = await _userManager.CreateAsync(user, registerProviderAccountDto.Password);
        if (result.Succeeded)
        {
            var provider = new Provider(Guid.NewGuid(), user.Id, registerProviderAccountDto.BusinessName);
            provider = await _providerRepository.CreateAsync(provider);

            if (provider != null)
            {
                user.ProviderId = provider.Id;
                //TODO check if this operation is successful
                await _userManager.UpdateAsync(user);
                return new ProviderDto()
                {
                    Id = provider.Id,
                    UserId = provider.UserId,
                    BusinessName = provider.BusinessName
                };
            }

            throw new InvalidOperationException("Unable to create provider");
        }

        throw new InvalidOperationException($"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");
    }

    public async Task<ProviderDto> ChangeProviderBusinessNameAsync(
        ChangeProviderBusinessNameDto changeProviderBusinessNameDto)
    {
        var providerId = _httpContextAccessor.HttpContext.User.FindFirst("ProviderId").Value;
        var provider = await _providerRepository.GetAsync(Guid.Parse(providerId));
        provider = await _providerRepository.ChangeBusinessNameAsync(provider,
            changeProviderBusinessNameDto.BusinessName);
        return new ProviderDto
        {
            Id = provider.Id,
            UserId = provider.UserId,
            BusinessName = provider.BusinessName
        };
    }
}