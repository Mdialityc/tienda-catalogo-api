using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Categories.Requests;

namespace tienda_catalogo_api.Endpoints.Categories;

public class UpdateCategoryEndpoint : Endpoint<UpdateCategoryRequest, Results<Ok, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public UpdateCategoryEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Put("/categories/{id}");
    }

    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(UpdateCategoryRequest req, CancellationToken ct)
    {
        var category = await _dbContext.Categories.FindAsync(req.Id, ct);

        if (category is null)
            return TypedResults.NotFound();

        category.Name = req.Name;
        category.Image = req.Image;
        category.ParentCategoryId = req.ParentId;
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}