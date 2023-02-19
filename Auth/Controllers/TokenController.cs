using DDD.AppUsers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace DDD.Controllers;

public class TokenController : OpenIddictBaseController
{

    [HttpPost("~/connect/token")]
    [IgnoreAntiforgeryToken]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = GetOpenIddictServerRequest();

        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
            throw new InvalidOperationException("The specified grant type is not supported.");

        // Retrieve the claims principal stored in the authorization code/refresh token.
        var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        // Retrieve the user profile corresponding to the authorization code/refresh token.
        var user = await UserManager.FindByIdAsync(result.Principal?.GetClaim(OpenIddictConstants.Claims.Subject) ??
                                                   string.Empty);
        if (user == null)
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The token is no longer valid."
                }));

        // Ensure the user is still allowed to sign in.
        if (!await SignInManager.CanSignInAsync(user))
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The token is no longer valid."
                }));

        return SignIn(result.Principal!, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}