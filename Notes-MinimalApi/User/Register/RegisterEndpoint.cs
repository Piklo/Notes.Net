using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Notes_MinimalApi.Database;
using Notes_MinimalApi.Sqlite;

namespace Notes_MinimalApi.User.Register;

internal static class RegisterEndpoint
{
    public static void MapRegisterEndpoint(this WebApplication app)
    {
        app.MapPost("/register", RegisterEndpoint.HandleAsync)
            .AllowAnonymous();
    }

    private static async Task<RegisterResponse> HandleAsync(HttpContext context, RegisterUserDto user, DatabaseAccess databaseAccess)
    {
        if (string.IsNullOrWhiteSpace(user.Login) || string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password))
        {
            return new RegisterResponse { Status = RegisterStatus.Failed };
        }

        var id = Guid.NewGuid();
        using var connection = databaseAccess.Connect();
        var hasher = new PasswordHasher<string>();
        var passwordHash = hasher.HashPassword(user.Login, user.Password);

        try
        {
            await connection.ExecuteAsync("INSERT INTO users (Id, Login, Email, PasswordHash) VALUES (@Id, @Login, @Email, @PasswordHash);",
                new { Id = id, user.Login, user.Email, PasswordHash = passwordHash });
        }
        catch (SqliteException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            if (ex.SqliteExtendedErrorCode == SqliteErrorCodes.SQLITE_CONSTRAINT_UNIQUE)
            {
                if (ex.Message.Contains("login", StringComparison.CurrentCultureIgnoreCase))
                {
                    return new RegisterResponse() { Status = RegisterStatus.LoginInUse };
                }

                if (ex.Message.Contains("email", StringComparison.CurrentCultureIgnoreCase))
                {
                    return new RegisterResponse() { Status = RegisterStatus.EmailInUse };
                }
            }

            return new RegisterResponse() { Status = RegisterStatus.Failed };
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new RegisterResponse() { Status = RegisterStatus.Failed };
        }


        context.Response.StatusCode = StatusCodes.Status201Created;
        return new RegisterResponse() { Status = RegisterStatus.Success };
    }
}
