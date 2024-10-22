using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Categories.Requests;
using tienda_catalogo_api.Endpoints.Categories.Responses;

namespace tienda_catalogo_api.Endpoints.Categories;

public class GetCategoryByIdEndpoint : Endpoint<GetCategoryByIdRequest, Results<Ok<CategoryResponse>, NotFound, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetCategoryByIdEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/categories/{id}");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<CategoryResponse>, NotFound, ProblemDetails>> ExecuteAsync(GetCategoryByIdRequest req, CancellationToken ct)
    {
        var category = await _dbContext.Categories.FindAsync(req.Id, ct);

        if (category is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(MapCategoryToResponse(category));
    }
    
    private CategoryResponse MapCategoryToResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Image = category.Image,
            Name = category.Name,
            SubCategories = category.Subcategories.Select(MapCategoryToResponse)
        };
    }
}