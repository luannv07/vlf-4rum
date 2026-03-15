// Services/Implementations/AuthService.cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using vlf_4rum.Data;
using vlf_4rum.Models;
using VlfForum.Services.Interfaces;
using Vlf4rum.Models;
using Vlf4rum.Constants;

namespace VlfForum.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly IHttpContextAccessor _httpContext;

    public AuthService(AppDbContext db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    // ── Register ──────────────────────────────────────────
    public async Task<(bool Success, string? Error)> RegisterAsync(
        string username, string email, string password)
    {
        // 1. Kiểm tra trùng
        if (await _db.Users.AnyAsync(u => u.Username == username))
            return (false, "Tên tài khoản đã được sử dụng");

        if (await _db.Users.AnyAsync(u => u.Email == email))
            return (false, "Email đã được sử dụng");

        // 2. Hash password
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        // 3. Tạo user
        var user = new User
        {
            Username = username,
            Email = email,
            PasswordHash = hash,
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        // 4. Gán role Member mặc định
        var memberRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == AppRoles.Member);
        if (memberRole != null)
        {
            _db.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = memberRole.Id,
            });
            await _db.SaveChangesAsync();
        }

        return (true, null);
    }

    // ── Login ─────────────────────────────────────────────
    public async Task<(bool Success, string? Error)> LoginAsync(
        string username, string password, bool rememberMe)
    {
        // 1. Tìm user
        var user = await _db.Users
            .Include(u => u.UserRole)
                .ThenInclude(ur => ur!.Role)
                    .ThenInclude(r => r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
            .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user == null)
            return (false, "Tên tài khoản hoặc mật khẩu không chính xác");

        // 2. Kiểm tra active
        if (!user.IsActive)
            return (false, "Tài khoản đã bị khóa");

        // 3. Verify password
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return (false, "Tên tài khoản hoặc mật khẩu không chính xác");

        // 4. Build claims
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name,           user.Username),
            new(ClaimTypes.Email,          user.Email),
        };

        if (!string.IsNullOrEmpty(user.AvatarLink))
            claims.Add(new Claim("AvatarLink", user.AvatarLink));

        // Role
        if (user.UserRole?.Role != null)
            claims.Add(new Claim(ClaimTypes.Role, user.UserRole.Role.Name));

        // Effective permissions = từ role + từ user_permission
        var rolePermissions = user.UserRole?.Role?.RolePermissions
            .Select(rp => rp.Permission.Name)
            .ToHashSet() ?? new HashSet<string>();

        var granted = user.UserPermissions
            .Where(up => up.IsGranted)
            .Select(up => up.Permission.Name)
            .ToHashSet();

        var revoked = user.UserPermissions
            .Where(up => !up.IsGranted)
            .Select(up => up.Permission.Name)
            .ToHashSet();

        var effectivePermissions = rolePermissions
            .Union(granted)
            .Except(revoked);

        foreach (var perm in effectivePermissions)
            claims.Add(new Claim("permission", perm));

        // 5. Sign in
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await _httpContext.HttpContext!.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = rememberMe }
        );

        // 6. Cập nhật LastLoginAt
        user.LastLoginAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return (true, null);
    }

    public async Task LogoutAsync()
    {
        await _httpContext.HttpContext!.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );
    }
}