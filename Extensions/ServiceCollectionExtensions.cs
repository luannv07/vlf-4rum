using VlfForum.Services.Interfaces;
using VlfForum.Services.Implementations;

namespace VlfForum.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}