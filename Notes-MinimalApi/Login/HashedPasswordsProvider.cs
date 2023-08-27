using Dapper;
using Notes_MinimalApi.Database;

namespace Notes_MinimalApi.Login;

internal sealed class HashedPasswordsProvider
{
    private readonly DatabaseAccess databaseAccess;

    public HashedPasswordsProvider(DatabaseAccess databaseAccess)
    {
        this.databaseAccess = databaseAccess;
    }

    public async Task<string?> GetHashedPasswordAsync(string login)
    {
        using var connection = databaseAccess.Connect();

        var hashedPassword = await connection.QueryFirstOrDefaultAsync<string>("SELECT PasswordHash FROM Users WHERE Login = @Login", new { Login = login });

        return hashedPassword;
    }
}
