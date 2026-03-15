using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vlf_4rum.Models;
using VlfForum.Controllers;
using VlfForum.Services.Interfaces;

[AllowAnonymous]
public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult Index(int statusCode) => statusCode switch
    {
        404 => View("~/Views/Shared/NotFound.cshtml"),
        403 => View("~/Views/Shared/AccessDenied.cshtml"),
        _ => View("~/Views/Shared/Error.cshtml")
    };
}