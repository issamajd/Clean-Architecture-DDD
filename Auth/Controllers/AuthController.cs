using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace DDD.Controllers;

public class AuthController : Controller
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;


    public AuthController(
        IOpenIddictApplicationManager applicationManager, IOpenIddictScopeManager scopeManager)
    {
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
    }

    [HttpPost, Route("~/connect/token"), IgnoreAntiforgeryToken, Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (!request.IsClientCredentialsGrantType())
            throw new NotImplementedException("The specified grant type is not implemented.");
        // Note: the client credentials are automatically validated by OpenIddict:
        // if client_id or client_secret are invalid, this action won't be invoked.

        var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        // Subject (sub) is a required field, we use the client id as the subject identifier here.
        identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId ?? throw new InvalidOperationException());

        // Add some claim, don't forget to add destination otherwise it won't be added to the access token.
        identity.AddClaim("some-claim", "some-value", OpenIddictConstants.Destinations.AccessToken);

        return SignIn(new ClaimsPrincipal(identity).SetScopes(request.GetScopes()), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}