using ClientServiceRazor.Features.Users.Models;
using ClientServiceRazor.Features.Users.Services;

namespace ClientServiceRazor.Features.Users.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserService userService)
    {
        if (context.Session.TryGetValue("UserId", out var bytes))
        {
            var userId = BitConverter.ToUInt32(bytes);
            var user = await userService.GetUserByIdAsync(userId);

            if (user != null)
            {
                context.Items["User"] = user;
            }
        }

        var path = context.Request.Path.Value ?? "";
        var currentUser = context.Items["User"] as User;

        var protectedPaths = new List<string>
        {
            "/Clients",
            "/Users/List",
        };

        if (protectedPaths.Any(p => path != null && path.StartsWith(p)))
        {
            if (currentUser == null)
            {
                context.Response.Redirect("/Users/Login");
                return;
            }
        }

        var protectedPathsRoles = new Dictionary<string, string[]>
        {
            { "/Users/List", ["Admin"] },
        };

        foreach (var entry in protectedPathsRoles)
        {
            if (path.StartsWith(entry.Key, StringComparison.OrdinalIgnoreCase))
            {
                var role = currentUser?.Role;

                if (role is null)
                {
                    context.Response.Redirect("/Users/Login");
                    return;
                }

                if (!entry.Value.Contains(role.Name))
                {
                    context.Response.Redirect("/Users/AccessDenied");
                    return;
                }
            }
        }

        await _next(context);
    }
}

public static class AuthMiddlewareExtensions
{
    public static IApplicationBuilder UseAuth(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthMiddleware>();
    }
}