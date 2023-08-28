using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Notes_MinimalApi.Database;
using Notes_MinimalApi.Sqlite;

namespace Notes_MinimalApi.Register;

internal sealed class RegisterAccountProvider
{
    private readonly DatabaseAccess databaseAccess;

    public RegisterAccountProvider(DatabaseAccess databaseAccess)
    {
        this.databaseAccess = databaseAccess;
    }

    public async Task<RegisterStatus> TryRegisterAccountAsync(RegisterUserDto user)
    {
        var id = Guid.NewGuid();
        using var connection = databaseAccess.Connect();
        var hasher = new PasswordHasher<string>();
        var passwordHash = hasher.HashPassword(user.Login, user.Password);

        try
        {
            await connection.ExecuteAsync("INSERT INTO users (Id, Login, Email, PasswordHash) VALUES (@Id, @Login, @Email, @PasswordHash);", new { Id = id, Login = user.Login, Email = user.Email, PasswordHash = passwordHash });
        }
        catch (SqliteException ex)
        {
            if (ex.SqliteExtendedErrorCode == SqliteErrorCodes.SQLITE_CONSTRAINT_UNIQUE)
            {
                if (ex.Message.Contains("login", StringComparison.CurrentCultureIgnoreCase))
                {
                    return RegisterStatus.LoginInUse;
                }

                if (ex.Message.Contains("email", StringComparison.CurrentCultureIgnoreCase))
                {
                    return RegisterStatus.EmailInUse;
                }
            }

            return RegisterStatus.Failed;
        }
        catch (Exception)
        {
            return RegisterStatus.Failed;
        }

        return RegisterStatus.Success;
    }
}
