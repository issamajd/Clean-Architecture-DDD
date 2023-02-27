using DDD.Identity.Helpers;
using DDD.Identity.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace DDD.Identity.Controllers;

public class AuthorizeController : OpenIddictBaseController
{
    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Authorize()
    {
        var request = GetOpenIddictServerRequest();

        // Try to retrieve the user principal stored in the authentication cookie and redirect
        // the user agent to the login page (or to an external provider) in the following cases:
        //
        //  - If the user principal can't be extracted or the cookie is too old.
        //  - If a max_age parameter was provided and the authentication cookie is not considered "fresh" enough.
        var result = await HttpContext.AuthenticateAsync();

        // If the user principal can't be extracted, redirect the user to the login page.
        if (!result.Succeeded ||
            request.MaxAge != null && result.Properties?.IssuedUtc != null
                                   && DateTimeOffset.UtcNow - result.Properties?.IssuedUtc >
                                   TimeSpan.FromSeconds(request.MaxAge.Value))
        {
            var parameters = Request.HasFormContentType
                ? Request.Form.ToList()
                : Request.Query.ToList();

            return Challenge(
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + QueryString.Create(parameters)
                });
        }

        var user = await UserManager.GetUserAsync(result.Principal) ??
                   throw new InvalidOperationException("The user details cannot be retrieved");

        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId ?? string.Empty) ??
                          throw new InvalidOperationException(
                              "Details concerning the calling client application cannot be found.");

        var applicationId = (await ApplicationManager.GetIdAsync(application))!;
        // Retrieve the permanent authorizations associated with the user and the calling client application.
        var authorizations = await AuthorizationManager.FindAsync(
            subject: user.Id.ToString(),
            client: applicationId,
            status: Statuses.Valid,
            type: AuthorizationTypes.Permanent,
            scopes: request.GetScopes()
        ).ToListAsync();

        switch (await ApplicationManager.GetConsentTypeAsync(application))
        {
            // If the consent is external (e.g when authorizations are granted by a sysadmin),
            // immediately return an error if no authorization can be found in the database.
            case ConsentTypes.External when !authorizations.Any():
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "The logged in user is not allowed to access this client application."
                    }));
            // If the consent is implicit or if an authorization was found,
            // return an authorization response without displaying the consent form.
            case ConsentTypes.Implicit:
            case ConsentTypes.External when authorizations.Any():
            case ConsentTypes.Explicit when authorizations.Any() && !request.HasPrompt(Prompts.Consent):

                var claimsPrincipal = await CreateClaimsPrincipalWithClaims(user);
                claimsPrincipal =
                    await AddAuthorizationToIdentity(claimsPrincipal, request, user, applicationId, authorizations);
                claimsPrincipal.SetDestinations(GetDestinations);
                return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // At this point, no authorization was found in the database and an error must be returned
            // if the client application specified prompt=none in the authorization request.
            case ConsentTypes.Explicit when request.HasPrompt(Prompts.None):
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                            "Interactive user consent is required."
                    }));
            // In every other case, render the consent form.
            default:
                return View(new AuthorizeViewModel
                {
                    ApplicationName = await ApplicationManager.GetLocalizedDisplayNameAsync(application),
                    Scope = request.Scope
                });
        }
    }

    [Authorize]
    [HttpPost("~/connect/authorize/callback"), ValidateAntiForgeryToken]
    public async Task<IActionResult> HandleCallbackAsync()
    {
        if (await HasFormValueAsync("submit.Deny"))
        {
            // Notify OpenIddict that the authorization grant has been denied by the resource owner
            // to redirect the user agent to the client application using the appropriate response_mode.
            return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        var request = GetOpenIddictServerRequest();

        var user = await UserManager.GetUserAsync(User) ??
                   throw new InvalidOperationException("The user details cannot be retrieved.");
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId ?? string.Empty) ??
                          throw new InvalidOperationException(
                              "Details concerning the calling client application cannot be found.");
        var applicationId = (await ApplicationManager.GetIdAsync(application))!;
        var authorizations = await AuthorizationManager.FindAsync(
            subject: user.Id.ToString(),
            client: applicationId,
            status: Statuses.Valid,
            type: AuthorizationTypes.Permanent,
            scopes: request.GetScopes()
        ).ToListAsync();

        // Note: the same check is already made in the other action but is repeated
        // here to ensure a malicious user can't abuse this POST-only endpoint and
        // force it to return a valid response without the external authorization.
        if (!authorizations.Any() && await ApplicationManager.HasConsentTypeAsync(application, ConsentTypes.External))
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.ConsentRequired,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "The logged in user is not allowed to access this client application."
                }));

        var claimsPrincipal = await CreateClaimsPrincipalWithClaims(user);
        claimsPrincipal =
            await AddAuthorizationToIdentity(claimsPrincipal, request, user, applicationId, authorizations);

        claimsPrincipal.SetDestinations(GetDestinations);
        return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}