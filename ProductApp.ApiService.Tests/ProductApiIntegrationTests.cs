using Xunit;
using Testcontainers.PostgreSql;
using Dapper;
using ProductApi.Models;
using ProductApi.Data;
using Microsoft.Extensions.Configuration;

public class ProductApiIntegrationTests : IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgContainer;
    private ProductRepository? _repo;

    public ProductApiIntegrationTests()
    {
        _pgContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("postgres")
            .WithPassword("password")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _pgContainer.StartAsync();

        // Criar tabela
        using var conn = new Npgsql.NpgsqlConnection(_pgContainer.GetConnectionString());
        await conn.ExecuteAsync(@"
            CREATE TABLE products (
                id SERIAL PRIMARY KEY,
                name TEXT NOT NULL,
                price NUMERIC(10,2) NOT NULL,
                description TEXT
            );
        ");

        // Simula IConfiguration
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Postgres"] = _pgContainer.GetConnectionString()
            }).Build();

        _repo = new ProductRepository(config);
    }

    public async Task DisposeAsync()
    {
        await _pgContainer.DisposeAsync();
    }

    [Fact]
    public async Task CanInsertAndRetrieveProduct()
    {
        Assert.NotNull(_repo);

        var product = new Product
        {
            Name = "Test Product",
            Price = 9.99m,
            Description = "Integration Test"
        };

        var newId = await _repo!.CreateAsync(product);

        var retrieved = await _repo.GetByIdAsync(newId);

        Assert.NotNull(retrieved);
        Assert.Equal("Test Product", retrieved!.Name);
    }

    [Fact]
    public async Task CanRetrieveAllProducts()
    {
        Assert.NotNull(_repo);

        // Inserir dois produtos
        await _repo!.CreateAsync(new Product { Name = "P1", Price = 1.0m, Description = "D1" });
        await _repo.CreateAsync(new Product { Name = "P2", Price = 2.0m, Description = "D2" });

        var all = (await _repo.GetAllAsync()).ToList();

        Assert.Equal(2, all.Count);
        Assert.Contains(all, p => p.Name == "P1");
        Assert.Contains(all, p => p.Name == "P2");
    }

    [Fact]
    public async Task CanUpdateProduct()
    {
        Assert.NotNull(_repo);

        var id = await _repo!.CreateAsync(new Product
        {
            Name = "Original",
            Price = 1.0m,
            Description = "Desc"
        });

        var updated = new Product
        {
            Id = id,
            Name = "Updated",
            Price = 5.5m,
            Description = "New Desc"
        };

        await _repo.UpdateAsync(updated);
        var afterUpdate = await _repo.GetByIdAsync(id);

        Assert.NotNull(afterUpdate);
        Assert.Equal("Updated", afterUpdate!.Name);
        Assert.Equal(5.5m, afterUpdate.Price);
    }

    [Fact]
    public async Task CanDeleteProduct()
    {
        Assert.NotNull(_repo);

        var id = await _repo!.CreateAsync(new Product
        {
            Name = "To Delete",
            Price = 3.3m,
            Description = "Temp"
        });

        await _repo.DeleteAsync(id);
        var deleted = await _repo.GetByIdAsync(id);

        Assert.Null(deleted);
    }



}
