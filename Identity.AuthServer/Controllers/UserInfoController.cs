using DDD.Identity.AppUsers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace DDD.Identity.Controllers;

public class UserInfoController : OpenIddictBaseController
{
    
    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    [HttpGet("~/connect/userinfo"), HttpPost("~/connect/userinfo"), Produces("application/json")]
    public async Task<IActionResult> Userinfo()
    {
        var user = await UserManager.FindByIdAsync(User.GetClaim(Claims.Subject)!);
        if (user == null)
            return Challenge(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidToken,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The specified access token is bound to an account that no longer exists."
                }));
        var claims = new Dictionary<string, object>()
        {
            [Claims.Subject] = await UserManager.GetUserIdAsync(user)
        };
        if (User.HasScope(Scopes.Email))
        {
            claims[Claims.Email] = (await UserManager.GetEmailAsync(user))!;
            claims[Claims.EmailVerified] = await UserManager.IsEmailConfirmedAsync(user);
        }

        if (User.HasScope(Scopes.Phone))
        {
            claims[Claims.PhoneNumber] = (await UserManager.GetPhoneNumberAsync(user))!;
            claims[Claims.PhoneNumberVerified] = await UserManager.IsPhoneNumberConfirmedAsync(user);
        }

        if (User.HasScope(Scopes.Roles))
        {
            claims[Claims.Role] = await UserManager.GetRolesAsync(user);
        }
        return Ok(claims);
    }
}