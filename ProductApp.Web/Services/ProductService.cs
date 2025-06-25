using ProductApp.Models;

namespace ProductApp.Services;

public class ProductService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public ProductService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config["ApiBaseUrl"];
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<Product>>($"{_baseUrl}/products") ?? [];
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Product>($"{_baseUrl}/products/{id}");
    }

    public async Task CreateAsync(Product p)
    {
        await _http.PostAsJsonAsync($"{_baseUrl}/products", p);
    }

    public async Task UpdateAsync(Product p)
    {
        await _http.PutAsJsonAsync($"{_baseUrl}/products/{p.Id}", p);
    }

    public async Task DeleteAsync(int id)
    {
        await _http.DeleteAsync($"{_baseUrl}/products/{id}");
    }
}


