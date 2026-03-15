using VlfForum.Middleware;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalException(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();

        return app;
    }
}