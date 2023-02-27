using AutoMapper;
using DDD.AppUsers;
using DDD.Customers;
using DDD.SeedWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DDD.Providers;

public class ProviderAppService : ApplicationService, IProviderAppService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProviderAppService(IUnitOfWork unitOfWork,
        IMapper mapper,
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProviderDto> GetAsync(Guid id)
    {
        var providerRepository = UnitOfWork.Repository<IProviderRepository, Provider>();
        var provider = await providerRepository.GetAsync(provider => provider.Id == id);
        return Mapper.Map<ProviderDto>(provider);
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
            return Mapper.Map<ProviderDto>(provider);
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
        var providerId = _httpContextAccessor.HttpContext.User.FindFirst("provider_id")?.Value;
        var providerRepository = UnitOfWork.Repository<IProviderRepository, Provider>();

        var provider = await providerRepository.GetAsync(provider => providerId != null
                                                                     && provider.Id == Guid.Parse(providerId));
        provider.BusinessName = changeProviderBusinessNameDto.BusinessName;
        provider = providerRepository.Update(provider);

        await UnitOfWork.SaveChangesAsync();
        return Mapper.Map<ProviderDto>(provider);
    }
}