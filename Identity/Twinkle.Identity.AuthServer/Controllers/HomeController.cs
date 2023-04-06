using Microsoft.AspNetCore.Mvc;

namespace Twinkle.Identity.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}