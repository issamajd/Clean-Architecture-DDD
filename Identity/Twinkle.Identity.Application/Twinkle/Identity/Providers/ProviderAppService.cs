using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Twinkle.Identity.AppUsers;
using Twinkle.SeedWork;

namespace Twinkle.Identity.Providers;

public class ProviderAppService : ApplicationService, IProviderAppService
{
    private readonly IProviderRepository _providerRepository;
    private readonly IIdentityService<Guid> _identityService;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<ProviderAppService> _logger;

    public ProviderAppService(IProviderRepository providerRepository,
        IIdentityService<Guid> identityService,
        UserManager<AppUser> userManager,
        ILogger<ProviderAppService> logger)
    {
        _providerRepository = providerRepository;
        _identityService = identityService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ProviderDto> GetAsync(Guid id)
    {
        var provider = await _providerRepository.GetAsync(provider => provider.Id == id);
        return provider.Adapt<ProviderDto>();
    }

    public async Task<ProviderDto> CreateAsync(RegisterProviderAccountDto registerProviderAccountDto)
    {
        var user = new AppUser(id: Guid.NewGuid(),
            username: registerProviderAccountDto.Username,
            email: registerProviderAccountDto.Email);

        //TODO move this logic to ProviderManager
        var result = await _userManager.CreateAsync(user, registerProviderAccountDto.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");

        result = await _userManager.AddToRoleAsync(user, Roles.Provider);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Unable to add role user: {result.Errors.FirstOrDefault()?.Description}");

        var provider = new Provider(id: Guid.NewGuid(), userId: user.Id,
            businessName: registerProviderAccountDto.BusinessName);
        provider = await _providerRepository.AddAsync(provider);
        return provider.Adapt<ProviderDto>();
    }

    public async Task<ProviderDto> ChangeProviderBusinessNameAsync(
        ChangeProviderBusinessNameDto changeProviderBusinessNameDto)
    {
        var userId = _identityService.GetUserId();

        var provider = await _providerRepository.GetAsync(provider => provider.UserId == userId);
        provider.BusinessName = changeProviderBusinessNameDto.BusinessName;
        provider = _providerRepository.Update(provider);

        return provider.Adapt<ProviderDto>();
    }
}