using System.Security.Claims;
using DDD.AppUsers;
using DDD.Customers;
using DDD.Helpers;
using DDD.Providers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace DDD.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public abstract class OpenIddictBaseController : Controller
{
    protected IOpenIddictApplicationManager ApplicationManager =>
        HttpContext.RequestServices.GetService<IOpenIddictApplicationManager>()!;

    protected IOpenIddictAuthorizationManager AuthorizationManager =>
        HttpContext.RequestServices.GetService<IOpenIddictAuthorizationManager>()!;

    protected IOpenIddictScopeManager ScopeManager =>
        HttpContext.RequestServices.GetService<IOpenIddictScopeManager>()!;

    protected UserManager<AppUser> UserManager =>
        HttpContext.RequestServices.GetService<UserManager<AppUser>>()!;

    protected SignInManager<AppUser> SignInManager =>
        HttpContext.RequestServices.GetService<SignInManager<AppUser>>()!;

    protected ICustomerRepository CustomerRepository =>
        HttpContext.RequestServices.GetService<ICustomerRepository>()!;

    protected IProviderRepository ProviderRepository =>
        HttpContext.RequestServices.GetService<IProviderRepository>()!;


    protected OpenIddictRequest GetOpenIddictServerRequest()
    {
        return HttpContext.GetOpenIddictServerRequest() ??
               throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
    }

    protected async Task<ClaimsPrincipal> CreateClaimsPrincipalWithClaims(AppUser user)
    {
        var claimsPrincipal = await SignInManager.CreateUserPrincipalAsync(user);
        claimsPrincipal.SetClaim(Claims.Subject, user.Id.ToString());
        
        var customer = await CustomerRepository.FindAsync(x => x.UserId == user.Id);
        var provider = await ProviderRepository.FindAsync(x => x.UserId == user.Id);
        
        //TODO use Domain.Shared const values
        if (customer != null)
            claimsPrincipal.SetClaim("customer_id", customer.Id.ToString());
        if (provider != null)
            claimsPrincipal.SetClaim("provider_id", provider.Id.ToString());
        return claimsPrincipal;
    }

    protected async Task<ClaimsPrincipal> AddAuthorizationToIdentity(ClaimsPrincipal claimsPrincipal,
        OpenIddictRequest request,
        AppUser user,
        string applicationId,
        IEnumerable<object> authorizations)
    {
        // Note: in this sample, the granted scopes match the requested scope
        // but you may want to allow the user to uncheck specific scopes.
        // For that, simply restrict the list of scopes before calling SetScopes.
        claimsPrincipal.SetScopes(request.GetScopes());
        claimsPrincipal.SetResources(await ScopeManager.ListResourcesAsync(claimsPrincipal.GetScopes()).ToListAsync());

        // Automatically create a permanent authorization to avoid requiring explicit consent
        // for future authorization or token requests containing the same scopes.
        var authorization = authorizations.LastOrDefault();
        authorization ??= await AuthorizationManager.CreateAsync(
            principal: claimsPrincipal,
            subject: await UserManager.GetUserIdAsync(user),
            client: applicationId,
            type: AuthorizationTypes.Permanent,
            scopes: claimsPrincipal.GetScopes());

        claimsPrincipal.SetAuthorizationId(await AuthorizationManager.GetIdAsync(authorization));
        return claimsPrincipal;
    }

    [Authorize(AuthenticationSchemes = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)]
    protected static IEnumerable<string> GetDestinations(Claim claim)
    {
        // Note: by default, claims are NOT automatically included in the access and identity tokens.
        // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
        // whether they should be included in access tokens, in identity tokens or in both.

        switch (claim.Type)
        {
            case Claims.Name:
                yield return Destinations.AccessToken;

                if (claim.Subject != null && claim.Subject.HasScope(Permissions.Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject != null && claim.Subject.HasScope(Permissions.Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject != null && claim.Subject.HasScope(Permissions.Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            // Never include the security stamp in the access and identity tokens, as it's a secret value.
            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }

    protected async Task<bool> HasFormValueAsync(string name)
    {
        if (!Request.HasFormContentType) return false;
        var form = await Request.ReadFormAsync();
        return !string.IsNullOrEmpty(form[name]);
    }
}