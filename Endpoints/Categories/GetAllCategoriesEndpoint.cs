using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Categories.Responses;

namespace tienda_catalogo_api.Endpoints.Categories;

public class GetAllCategoriesEndpoint : EndpointWithoutRequest<Results<Ok<IEnumerable<CategoryResponse>>, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetAllCategoriesEndpoint(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/categories");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<IEnumerable<CategoryResponse>>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
        var categories = _dbContext.Categories
            .Include(x => x.Subcategories)
            .Where(x => x.ParentCategoryId == null)
            .OrderBy(x => x.Id).ToList();
        
        
        return TypedResults.Ok(categories.Select(MapCategoryToResponse));
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