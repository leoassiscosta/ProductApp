using Dapper;
using Npgsql;
using System.Net;

namespace ProductApi.Infrastructure;

using Dapper;
using Npgsql;

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

        const string checkSeedSql = "SELECT COUNT(*) FROM products;";
        var count = await connection.ExecuteScalarAsync<int>(checkSeedSql);

        if (count == 0)
        {
            const string insertSql = """
                INSERT INTO products (name, price, description) VALUES
                ('Wireless Mouse', 25.99, 'Compact wireless mouse with ergonomic design'),
                ('Mechanical Keyboard', 79.50, 'RGB backlit keyboard with blue switches'),
                ('27\" Monitor', 199.99, 'Full HD LED monitor with HDMI input'),
                ('USB-C Hub', 39.90, 'Multiport adapter with HDMI, USB 3.0 and SD card reader'),
                ('Noise Cancelling Headphones', 149.00, 'Over-ear headphones with active noise cancellation');
            """;

            await connection.ExecuteAsync(insertSql);
        }
    }
}

