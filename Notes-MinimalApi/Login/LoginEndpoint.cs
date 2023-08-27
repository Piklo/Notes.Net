using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Notes_MinimalApi.Login;

internal static class LoginEndpoint
{
    public static async Task<LoginResponse> HandleLogin(HttpContext context, LoginUserDto userDto, HashedPasswordsProvider hashedPasswordsProvider)
    {
        var hashedPassword = await hashedPasswordsProvider.GetHashedPasswordAsync(userDto.Login);
        var hasher = new PasswordHasher<string>();
        var newHashedPassword = hasher.HashPassword(userDto.Login, userDto.Password);

        if (hashedPassword is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new LoginResponse() { Status = LoginStatus.UserNotFound };
        }

        var result = hasher.VerifyHashedPassword(userDto.Login, hashedPassword, newHashedPassword);

        if (result == PasswordVerificationResult.Failed)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new LoginResponse() { Status = LoginStatus.Failed };
        }

        var claims = new List<Claim>()
        {
            new Claim("userLogin", userDto.Login)
        };

        var identity = new ClaimsIdentity(claims, Program.AuthSchema);
        var user = new ClaimsPrincipal(identity);
        await context.SignInAsync(user);

        return new LoginResponse() { Status = LoginStatus.Success };
    }
}
