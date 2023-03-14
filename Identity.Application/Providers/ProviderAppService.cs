using Core;
using DDD.Identity.AppUsers;
using DDD.Identity.SeedWork;
using Mapster;
using Microsoft.AspNetCore.Identity;

namespace DDD.Identity.Providers;

public class ProviderAppService : ApplicationService, IProviderAppService
{
    private readonly IIdentityService<Guid> _identityService;
    private readonly UserManager<AppUser> _userManager;

    public ProviderAppService(IUnitOfWork unitOfWork,
        IIdentityService<Guid> identityService,
        UserManager<AppUser> userManager) : base(unitOfWork)
    {
        _identityService = identityService;
        _userManager = userManager;
    }

    public async Task<ProviderDto> GetAsync(Guid id)
    {
        var providerRepository = UnitOfWork.Repository<IProviderRepository, Provider>();
        var provider = await providerRepository.GetAsync(provider => provider.Id == id);
        return provider.Adapt<ProviderDto>();
    }

    public async Task<ProviderDto> CreateAsync(RegisterProviderAccountDto registerProviderAccountDto)
    {
        var user = new AppUser(id: Guid.NewGuid(),
            username: registerProviderAccountDto.Username,
            email: registerProviderAccountDto.Email);

        await UnitOfWork.BeginAsync();
        try
        {
            var result = await _userManager.CreateAsync(user, registerProviderAccountDto.Password);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Unable to create a user: {result.Errors.FirstOrDefault()?.Description}");

            result = await _userManager.AddToRoleAsync(user, Roles.Provider);
            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Unable to add role user: {result.Errors.FirstOrDefault()?.Description}");

            var providerRepository = UnitOfWork.Repository<IProviderRepository, Provider>();

            var provider = new Provider(id: Guid.NewGuid(), userId: user.Id,
                businessName: registerProviderAccountDto.BusinessName);
            provider = await providerRepository.AddAsync(provider);

            await UnitOfWork.CompleteAsync();
            return provider.Adapt<ProviderDto>();
        }
        catch (Exception)
        {
            UnitOfWork.Cancel();
            throw;
        }
    }

    public async Task<ProviderDto> ChangeProviderBusinessNameAsync(
        ChangeProviderBusinessNameDto changeProviderBusinessNameDto)
    {
        var userId = _identityService.GetUserId();
        var providerRepository = UnitOfWork.Repository<IProviderRepository, Provider>();

        var provider = await providerRepository.GetAsync(provider =>  provider.UserId == userId);
        provider.BusinessName = changeProviderBusinessNameDto.BusinessName;
        provider = providerRepository.Update(provider);

        await UnitOfWork.SaveChangesAsync();
        return provider.Adapt<ProviderDto>();
    }
}