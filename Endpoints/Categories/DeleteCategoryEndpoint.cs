using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Categories.Requests;

namespace tienda_catalogo_api.Endpoints.Categories;

public class DeleteCategoryEndpoint : Endpoint<GetCategoryByIdRequest, Results<Ok, NotFound, ProblemDetails>>
{
    private readonly AppDbContext dbContext;

    public DeleteCategoryEndpoint(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public override void Configure()
    {
        Delete("/categories/{id}");
        Roles("Admin");
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(GetCategoryByIdRequest req, CancellationToken ct)
    {
        var category = await dbContext.Categories.FindAsync(req.Id, ct);

        if (category is null)
            return TypedResults.NotFound();

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}
