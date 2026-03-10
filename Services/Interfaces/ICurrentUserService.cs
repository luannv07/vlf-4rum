namespace VlfForum.Services.Interfaces;

public interface ICurrentUserService
{
    int? UserId { get; }

    string? Username { get; }
}