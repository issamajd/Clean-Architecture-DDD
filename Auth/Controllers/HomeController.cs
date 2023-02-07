using Microsoft.AspNetCore.Mvc;

namespace DDD.Controllers;

public class HomeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}