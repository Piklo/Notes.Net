using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Notes_MinimalApi.Database;
using System.Security.Claims;

namespace Notes_MinimalApi.Login;

internal static class LoginEndpoint
{
    public static async Task<LoginResponse> HandleLogin(HttpContext context, LoginUserDto userDto, HashedPasswordsProvider hashedPasswordsProvider, DatabaseAccess databaseAccess)
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

        using var connection = databaseAccess.Connect();

        var id = await connection.QueryFirstOrDefaultAsync<string>("SELECT Id FROM Users WHERE Login = @Login", new { Login = userDto.Login });

        if (id is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new LoginResponse() { Status = LoginStatus.UserNotFound };
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimConstants.UserId, id)
        };

        var identity = new ClaimsIdentity(claims, Constants.AuthSchema);
        var user = new ClaimsPrincipal(identity);
        await context.SignInAsync(user);

        return new LoginResponse() { Status = LoginStatus.Success };
    }
}
