using DDD.Identity.Providers;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ProviderDto> GetById(Guid id)
    {
        return await _providerAppService.GetAsync(id);
    }

    [Route("register")]
    [HttpPut]
    public async Task<ProviderDto> Register(RegisterProviderAccountDto registerProviderAccountDto)
    {
        return await _providerAppService.CreateAsync(registerProviderAccountDto);
    }

    [Route("change-business-name")]
    [HttpPost]
    [Authorize(Roles = Roles.Provider)]
    public async Task<ProviderDto> ChangeBusinessName(ChangeProviderBusinessNameDto changeProviderBusinessNameDto)
    {
        return await _providerAppService.ChangeProviderBusinessNameAsync(changeProviderBusinessNameDto);
    }
}