using Microsoft.AspNetCore.Mvc;
using VlfForum.Services.Interfaces;

namespace VlfForum.Controllers;

public class BaseController : Controller
{
    protected readonly ICurrentUserService CurrentUser;

    public BaseController(ICurrentUserService currentUser)
    {
        CurrentUser = currentUser;
    }
}