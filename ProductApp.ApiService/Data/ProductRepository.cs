using Dapper;
using Npgsql;
using ProductApi.Models;

namespace ProductApi.Data;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("Postgres")!;
    }

    private NpgsqlConnection GetConnection() => new(_connectionString);

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var conn = GetConnection();
        return await conn.QueryAsync<Product>("SELECT * FROM products");
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var conn = GetConnection();
        return await conn.QueryFirstOrDefaultAsync<Product>("SELECT * FROM products WHERE id = @id", new { id });
    }

    public async Task<int> CreateAsync(Product product)
    {
        using var conn = GetConnection();
        var sql = @"INSERT INTO products (name, price, description) 
                    VALUES (@Name, @Price, @Description) RETURNING id";
        return await conn.ExecuteScalarAsync<int>(sql, product);
    }

    public async Task UpdateAsync(Product product)
    {
        using var conn = GetConnection();
        var sql = @"UPDATE products SET name = @Name, price = @Price, description = @Description WHERE id = @Id";
        await conn.ExecuteAsync(sql, product);
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = GetConnection();
        await conn.ExecuteAsync("DELETE FROM products WHERE id = @id", new { id });
    }
}
