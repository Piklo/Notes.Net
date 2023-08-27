using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Notes_MinimalApi.Login;

internal static class LoginEndpoint
{
    public static async Task<string> HandleLogin(LoginUserDto userDto, HttpContext context)
    {
        if (!userDto.IsAuthenticated)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return "failed to login";
        }

        var claims = new List<Claim>()
        {
            new Claim("userLogin", userDto.Login)
        };

        var identity = new ClaimsIdentity(claims, Program.AuthSchema);
        var user = new ClaimsPrincipal(identity);
        await context.SignInAsync(user);

        return "logged in";
    }
}
