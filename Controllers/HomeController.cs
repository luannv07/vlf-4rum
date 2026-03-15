using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using vlf_4rum.Models;
using VlfForum.Controllers;
using VlfForum.Services.Interfaces;

namespace vlf_4rum.Controllers;

public class HomeController : BaseController
{
    public HomeController(ICurrentUserService currentUser) : base(currentUser)
    {
    }
    public IActionResult Test()
    {
        var userId = CurrentUser.UserId;

        return View(userId);
    }

    public IActionResult Index()
    {
        return View();
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
