using Dapper;
using Npgsql;
using System.Net;

namespace ProductApi.Infrastructure;

public static class DbInitializer
{
    public static async Task EnsureDatabaseCreatedAsync(string connectionString)
    {
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        const string createTableSql = """
            CREATE TABLE IF NOT EXISTS products (
                id SERIAL PRIMARY KEY,
                name TEXT NOT NULL,
                price NUMERIC(10,2) NOT NULL,
                description TEXT
            );
        """;

        await connection.ExecuteAsync(createTableSql);
    }
}
