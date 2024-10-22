using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Products.Requests;

namespace tienda_catalogo_api.Endpoints.Products;

public class DeleteProductEndpoint(AppDbContext dbContext) : Endpoint<GetProductByIdRequest, Results<Ok, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Delete("/products/{id}");
        Roles("Admin");
    }
    
    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(GetProductByIdRequest req, CancellationToken ct)
    {
        var p = await dbContext.Products.FindAsync(req.Id, ct);

        if (p is null)
            return TypedResults.NotFound();

        dbContext.Products.Remove(p);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}