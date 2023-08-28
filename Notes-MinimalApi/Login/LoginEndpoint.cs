using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Notes_MinimalApi.Login;

internal static class LoginEndpoint
{
    public static async Task<LoginResponse> HandleLogin(HttpContext context, LoginUserDto userDto, HashedPasswordsProvider hashedPasswordsProvider)
    {
        var hashedPassword = await hashedPasswordsProvider.GetHashedPasswordAsync(userDto.Login);

        if (hashedPassword is null)
        {
            return new LoginResponse() { Status = LoginStatus.UserNotFound };
        }

        var hasher = new PasswordHasher<string>();

        var result = hasher.VerifyHashedPassword(userDto.Login, hashedPassword, userDto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new LoginResponse() { Status = LoginStatus.WrongPassword };
        }

        var claims = new List<Claim>()
        {
            new Claim("userLogin", userDto.Login)
        };

        var identity = new ClaimsIdentity(claims, Constants.AuthSchema);
        var user = new ClaimsPrincipal(identity);
        await context.SignInAsync(user);

        return new LoginResponse() { Status = LoginStatus.Success };
    }
}
