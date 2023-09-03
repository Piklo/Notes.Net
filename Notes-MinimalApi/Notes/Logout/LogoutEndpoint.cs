using Microsoft.AspNetCore.Authentication;

namespace Notes_MinimalApi.Notes.Logout;

internal static class LogoutEndpoint
{
    public static void MapLogoutEndpoint(this WebApplication app)
    {
        app.MapPost("/logout", HandleAsync);
    }

    private static async Task HandleAsync(HttpContext context)
    {
        await context.SignOutAsync();
    }
}
