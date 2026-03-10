using System.Security.Claims;
using VlfForum.Services.Interfaces;

namespace VlfForum.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var claim = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return null;

            return int.Parse(claim.Value);
        }
    }

    public string? Username
    {
        get
        {
            return _httpContextAccessor
                .HttpContext?
                .User?
                .Identity?
                .Name;
        }
    }
}