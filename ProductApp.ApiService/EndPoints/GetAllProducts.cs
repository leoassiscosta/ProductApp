using FastEndpoints;
using ProductApi.Data;
using ProductApi.Models;

public class GetAllProductsEndpoint : EndpointWithoutRequest<IEnumerable<Product>>
{
    private readonly ProductRepository _repo;

    public GetAllProductsEndpoint(ProductRepository repo)
    {
        _repo = repo;
    }

    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var products = await _repo.GetAllAsync();
        await SendAsync(products);
    }
}
