using FastEndpoints;
using ProductApi.Data;
using ProductApi.Models;

public class GetProductByIdEndpoint : Endpoint<GetProductByIdRequest, Product>
{
    private readonly ProductRepository _repo;

    public GetProductByIdEndpoint(ProductRepository repo)
    {
        _repo = repo;
    }

    public override void Configure()
    {
        Get("/products/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetProductByIdRequest req, CancellationToken ct)
    {
        var product = await _repo.GetByIdAsync(req.Id);
        if (product is null)
            await SendNotFoundAsync();
        else
            await SendAsync(product);
    }
}

public class GetProductByIdRequest
{
    public int Id { get; set; }
}
