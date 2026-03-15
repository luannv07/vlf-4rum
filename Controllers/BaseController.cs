using Microsoft.AspNetCore.Mvc;
using VlfForum.Services.Interfaces;

namespace VlfForum.Controllers;

public abstract class BaseController : Controller
{
    protected readonly ICurrentUserService _currentUser;

    protected BaseController(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }
}