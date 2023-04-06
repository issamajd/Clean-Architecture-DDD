using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Twinkle.Identity.AppUsers;
using Twinkle.Identity.Helpers;
using Twinkle.Identity.ViewModels;

namespace Twinkle.Identity.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class AccountController : Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountController(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymousOnly]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        var model = new LoginViewModel()
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
            ReturnUrl = returnUrl
        };

        return View(model);
    }

    [AllowAnonymousOnly]
    [HttpPost]
    public IActionResult ExternalLogin(string provider, string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
            new { ReturnUrl = returnUrl });

        var properties =
            _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

        return new ChallengeResult(provider, properties);
    }

    [AllowAnonymousOnly]
    public async Task<IActionResult>
        ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        returnUrl ??= Url.Content("~/");
        var model = new LoginViewModel
        {
            ReturnUrl = returnUrl,
            ExternalLogins =
                (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
        };

        if (remoteError != null)
        {
            ModelState
                .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

            return View("Login", model);
        }

        // Get the login information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ModelState
                .AddModelError(string.Empty, "Error loading external login information.");

            return View("Login", model);
        }

        // If the user already has a login (i.e if there is a record in AspNetUserLogins
        // table) then sign-in the user with this external login provider
        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
            info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        // If there is no record in AspNetUserLogins table, the user may not have
        // a local account

        // Get the email claim value
        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (email == null)
        {
            // If we cannot find the user email we cannot continue
            ModelState
                .AddModelError(string.Empty, $"Email claim not received from: {info.LoginProvider}");
        }
        else
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState
                    .AddModelError(string.Empty, $"The user is not registered yet: {email}");
            }
            else
            {
                // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }

        return View("Login", model);
    }


    [HttpPost]
    [AllowAnonymousOnly]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid Credentials");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
        if (result.Succeeded) return Redirect(Url.IsLocalUrl(model.ReturnUrl) ? model.ReturnUrl : Url.Content("~/"));
        ModelState.AddModelError(string.Empty, "Invalid Credentials");
        return View(model);

    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}