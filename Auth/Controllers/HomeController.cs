using Microsoft.AspNetCore.Mvc;

namespace DDD.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}