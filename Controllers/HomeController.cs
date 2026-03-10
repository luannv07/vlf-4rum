using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using vlf_4rum.Models;

namespace vlf_4rum.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Content("Hello World from ASP.NET Core MVC running in Docker!");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
