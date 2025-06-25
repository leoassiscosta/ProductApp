using FastEndpoints;
using ProductApi.Data;
using ProductApi.Models;

public class UpdateProductEndpoint : Endpoint<UpdateProductRequest>
{
    private readonly ProductRepository _repo;

    public UpdateProductEndpoint(ProductRepository repo)
    {
        _repo = repo;
    }

    public override void Configure()
    {
        Put("/products/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var existing = await _repo.GetByIdAsync(req.Id);
        if (existing is null)
        {
            await SendNotFoundAsync();
            return;
        }

        var updated = new Product
        {
            Id = req.Id,
            Name = req.Name,
            Price = req.Price,
            Description = req.Description
        };

        await _repo.UpdateAsync(updated);
        await SendOkAsync();
    }
}

public class UpdateProductRequest : Product
{
    // Herda Id, Name, Price, Description
}
