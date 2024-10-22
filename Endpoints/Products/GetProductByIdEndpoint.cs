using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Products.Requests;

namespace tienda_catalogo_api.Endpoints.Products;

public class GetProductByIdEndpoint(AppDbContext dbContext) : Endpoint<GetProductByIdRequest, Results<Ok<Product>, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Get("/products/{id}");
        AllowAnonymous();
    }
    
    public override async Task<Results<Ok<Product>, NotFound, ProblemDetails>> ExecuteAsync(GetProductByIdRequest req, CancellationToken ct)
    {
        var p = await dbContext.Products.FindAsync(req.Id, ct);

        if (p is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(p);
    }
}