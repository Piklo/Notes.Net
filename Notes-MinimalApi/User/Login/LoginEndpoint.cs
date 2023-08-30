using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Notes_MinimalApi.Database;
using System.Security.Claims;

namespace Notes_MinimalApi.User.Login;

internal static class LoginEndpoint
{
    public static void MapLoginEndpoint(this WebApplication app)
    {
        app.MapPost("/login", LoginEndpoint.HandleAsync)
            .AllowAnonymous();
    }

    private static async Task<LoginResponse> HandleAsync(HttpContext context, LoginUserDto userDto, DatabaseAccess databaseAccess)
    {
        using var connection = databaseAccess.Connect();

        var userData = await connection.QueryFirstOrDefaultAsync<LoginUserData>("SELECT Id, PasswordHash FROM Users WHERE Login = @Login", new { userDto.Login });

        if (userData is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new LoginResponse() { Status = LoginStatus.UserNotFound };
        }

        var hasher = new PasswordHasher<string>();

        var result = hasher.VerifyHashedPassword(userDto.Login, userData.PasswordHash, userDto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return new LoginResponse() { Status = LoginStatus.WrongPassword };
        }

        var claims = new List<Claim>()
        {
            new Claim(ClaimConstants.UserId, userData.Id)
        };

        var identity = new ClaimsIdentity(claims, Constants.AuthSchema);
        var user = new ClaimsPrincipal(identity);
        await context.SignInAsync(user);

        return new LoginResponse() { Status = LoginStatus.Success };
    }
}
