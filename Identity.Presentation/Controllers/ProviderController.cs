using DDD.Authorization.AspNetCore;
using DDD.Identity.Permissions;
using DDD.Identity.Providers;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Identity.Presentation.Controllers;

[ApiController]
[Route("provider")]
public class ProviderController : ControllerBase
{
    private readonly IProviderAppService _providerAppService;

    public ProviderController(IProviderAppService providerAppService)
    {
        _providerAppService = providerAppService;
    }

    [HttpGet]
    [Permission(IdentityPermissions.Providers.Default)]
    public async Task<ProviderDto> GetById(Guid id) => await _providerAppService.GetAsync(id);

    [Route("register")]
    [HttpPut]
    public async Task<ProviderDto> Register(RegisterProviderAccountDto registerProviderAccountDto) =>
        await _providerAppService.CreateAsync(registerProviderAccountDto);

    [Route("change-business-name")]
    [HttpPost]
    [Permission(IdentityPermissions.Providers.Edit)]
    public async Task<ProviderDto> ChangeBusinessName(ChangeProviderBusinessNameDto changeProviderBusinessNameDto) =>
        await _providerAppService.ChangeProviderBusinessNameAsync(changeProviderBusinessNameDto);
}