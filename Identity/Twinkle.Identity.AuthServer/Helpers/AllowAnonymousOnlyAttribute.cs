using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Twinkle.Identity.Controllers;

namespace Twinkle.Identity.Helpers;

public class AllowAnonymousOnlyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            context.Result = new RedirectToActionResult(nameof(HomeController.Index), "Home", null);
        }
    }
}