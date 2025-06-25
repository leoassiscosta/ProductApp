using FastEndpoints;
using ProductApi.Data;
using ProductApi.Models;

public class CreateProductEndpoint : Endpoint<Product, int>
{
    private readonly ProductRepository _repo;

    public CreateProductEndpoint(ProductRepository repo)
    {
        _repo = repo;
    }

    public override void Configure()
    {
        Post("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Product req, CancellationToken ct)
    {
        var newId = await _repo.CreateAsync(req);
        await SendAsync(newId);
    }
}
