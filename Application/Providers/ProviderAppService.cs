using DDD.AppUsers;
using DDD.Customers;
using DDD.SeedWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace DDD.Providers;

public class ProviderAppService : ApplicationService, IProviderAppService
{
    private readonly IProviderRepository _providerRepository;
    private readonly AppUserManager _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProviderAppService(IUnitOfWork unitOfWork, IProviderRepository providerRepository,
        AppUserManager userManager,
        IHttpContextAccessor httpContextAccessor) : base(unitOfWork)
    {
        _providerRepository = providerRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProviderDto> GetAsync(Guid id)
    {
        var provider = await _providerRepository.GetAsync(provider => provider.Id == id);
        return new ProviderDto
        {
            BusinessName = provider.BusinessName, Id = provider.Id,
            UserId = provider.UserId
        };
    }

    public async Task<ProviderDto> CreateAsync(RegisterProviderAccountDto registerProviderAccountDto)
    {
        var user = new AppUser(id: Guid.NewGuid(), username: registerProviderAccountDto.Username);

        var result = await _userManager.AddUserWithRolesAsync(user, registerProviderAccountDto.Password,new[] {Roles.Provider});
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");
        
        var provider = new Provider(id: Guid.NewGuid(), userId: user.Id,
            businessName: registerProviderAccountDto.BusinessName);
        provider = await _providerRepository.InsertAsync(provider);
        await UnitOfWork.SaveChangesAsync();

        return new ProviderDto()
        {
            Id = provider.Id,
            UserId = provider.UserId,
            BusinessName = provider.BusinessName
        };
    }

    [Authorize(Roles = Roles.Provider)]
    public async Task<ProviderDto> ChangeProviderBusinessNameAsync(
        ChangeProviderBusinessNameDto changeProviderBusinessNameDto)
    {
        var providerId = _httpContextAccessor.HttpContext.User.FindFirst("provider_id")?.Value;
        var provider = await _providerRepository.GetAsync(provider => providerId != null
                                                                      && provider.Id == Guid.Parse(providerId));
        provider.BusinessName = changeProviderBusinessNameDto.BusinessName; 
        provider = _providerRepository.Update(provider);

        await UnitOfWork.SaveChangesAsync();
        return new ProviderDto
        {
            Id = provider.Id,
            UserId = provider.UserId,
            BusinessName = provider.BusinessName
        };
    }
}