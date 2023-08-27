using Microsoft.Data.Sqlite;
using System.Data;

namespace Notes_MinimalApi.Database;

internal sealed class DatabaseAccess
{
    private readonly IConfiguration config;

    public DatabaseAccess(IConfiguration config)
    {
        this.config = config;
    }

    public IDbConnection Connect()
    {
        return new SqliteConnection(config.GetConnectionString("Notes"));
    }

    //public async Task<IEnumerable<T>> LoadData<T, U>(string sql, U parameters)
    //{
    //    using var connection = new SqliteConnection(config.GetConnectionString("Notes"));

    //    return await connection.QueryAsync<T>(sql, parameters);
    //}

    //public async Task SaveData<T>(string sql, T parameters)
    //{
    //    using var connection = new SqliteConnection(config.GetConnectionString("Notes"));

    //    await connection.ExecuteAsync(sql, parameters);
    //}
}
