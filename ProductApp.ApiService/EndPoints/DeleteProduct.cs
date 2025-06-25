using FastEndpoints;
using ProductApi.Data;

public class DeleteProductEndpoint : Endpoint<DeleteProductRequest>
{
    private readonly ProductRepository _repo;

    public DeleteProductEndpoint(ProductRepository repo)
    {
        _repo = repo;
    }

    public override void Configure()
    {
        Delete("/products/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteProductRequest req, CancellationToken ct)
    {
        var existing = await _repo.GetByIdAsync(req.Id);
        if (existing is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await _repo.DeleteAsync(req.Id);
        await SendOkAsync();
    }
}

public class DeleteProductRequest
{
    public int Id { get; set; }
}
