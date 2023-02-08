using DDD.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace DDD.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly IAccountAppService _accountAppService;

    public AccountController(IAccountAppService accountAppService)
    {
        _accountAppService = accountAppService;
    }


    [Route("sign-in")]
    [HttpPost]
    public async Task<TokenDto> SignIn(SignInDto signInDto)
    {
        return await _accountAppService.SignIn(signInDto);
    }
}