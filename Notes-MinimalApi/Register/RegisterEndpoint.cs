using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Notes_MinimalApi.Database;
using Notes_MinimalApi.Sqlite;

namespace Notes_MinimalApi.Register;

internal static class RegisterEndpoint
{
    public static async Task<RegisterResponse> HandleRegister(HttpContext context, RegisterUserDto user, DatabaseAccess databaseAccess)
    {
        var id = Guid.NewGuid();
        using var connection = databaseAccess.Connect();
        var hasher = new PasswordHasher<string>();
        var passwordHash = hasher.HashPassword(user.Login, user.Password);

        try
        {
            await connection.ExecuteAsync("INSERT INTO users (Id, Login, Email, PasswordHash) VALUES (@Id, @Login, @Email, @PasswordHash);",
                new { Id = id, Login = user.Login, Email = user.Email, PasswordHash = passwordHash });
        }
        catch (SqliteException ex)
        {
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
            return new RegisterResponse() { Status = RegisterStatus.Failed };
        }


        return new RegisterResponse() { Status = RegisterStatus.Success };
    }
}
