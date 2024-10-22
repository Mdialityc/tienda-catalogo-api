using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Categories.Requests;

namespace tienda_catalogo_api.Endpoints.Categories;

public class CreateCategoryBulkEndpoint : Endpoint<IEnumerable<CreateCategoryRequest>, Results<Created, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public CreateCategoryBulkEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/categories");
        Roles("Admin");
    }

    public override async Task<Results<Created, ProblemDetails>> ExecuteAsync(IEnumerable<CreateCategoryRequest> req, CancellationToken ct)
    {
        var categories = req.Select(x => new Category
        {
            Image = x.Image.Trim(),
            Name = x.Name.Trim(),
            ParentCategoryId = x.ParentId
        });

        await _dbContext.Categories.AddRangeAsync(categories, ct);
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Created();
    }
}