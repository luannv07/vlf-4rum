namespace VlfForum.Services.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(string username, string email, string password);
    Task<(bool Success, string? Error)> LoginAsync(string username, string password, bool rememberMe);
    Task LogoutAsync();
}