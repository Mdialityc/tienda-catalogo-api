using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Commons.Responses;
using tienda_catalogo_api.Endpoints.Products.Requests;

namespace tienda_catalogo_api.Endpoints.Products;

public class SearchProductsEndpoint(AppDbContext dbContext) : Endpoint<SearchProductsRequest, Results<Ok<PaginatedResponse<Product>>, ProblemDetails>>
{
    public override void Configure()
    {
        Get("/products");
        AllowAnonymous();
    }
    
     public override async Task<Results<Ok<PaginatedResponse<Product>>, ProblemDetails>>
        ExecuteAsync(SearchProductsRequest req, CancellationToken ct)
    {
        var query = dbContext.Products
            .AsNoTracking().AsQueryable();

        // Filtering Section
        if (req.Ids?.Any() ?? false)
        {
            query = query.Where(x => req.Ids.Contains(x.Id));
        }

        if (req.Names?.Any() ?? false)
        {
            query = query.Where(x => req.Names.Any(y => x.Name.ToLower().Contains(y.ToLower().Trim())));
        }

        if (req.CategoryIds?.Any() ?? false)
        {
            query = query.Where(x => req.CategoryIds.Contains(x.CategoryId));
        }

        if (req.HasStock is not null)
        {
            query = query.Where(x => req.HasStock == x.HasStock);
        }

        if (req.Currency is not null)
        {
            query = query.Where(x => req.Currency == x.Currency);
        }

        if (req.HasSale is not null)
        {
            query = query.Where(x => req.HasSale == x.HasSale);
        }
        
        if (req.HasDiscount is not null)
        {
            query = query.Where(x => req.HasDiscount.Value ? x.DiscountAmount != null : x.DiscountAmount == null);
        }

        if (req.SalesStart is not null)
        {
            req.SalesStart = req.SalesStart.Value.ToUniversalTime();
            query = query.Where(x => x.SaleStart >= req.SalesStart);
        }

        if (req.SalesEnd is not null)
        {
            req.SalesEnd = req.SalesEnd.Value.ToUniversalTime();
            query = query.Where(x => x.SaleEnd <= req.SalesEnd);
        }
        

        if (req.MinimumPrice is not null)
        {
            query = query.Where(x => x.Price >= req.MinimumPrice);
        }

        if (req.MaximumPrice is not null)
        {
            query = query.Where(x => x.Price <= req.MaximumPrice);
        }
        
        if (req.MinimumDiscountAmount is not null)
        {
            query = query.Where(x => x.DiscountAmount >= req.MinimumDiscountAmount);
        }

        if (req.MaximumDiscountAmount is not null)
        {
            query = query.Where(x => x.DiscountAmount <= req.MaximumDiscountAmount);
        }

        var products = (await query.ToListAsync(ct)).AsEnumerable();

        // Sorting Section
        if (!string.IsNullOrEmpty(req.SortBy))
        {
            var propertyInfo = typeof(Product).GetProperty(req.SortBy);
            if (propertyInfo != null)
            {
                products = req?.IsDescending ?? false
                    ? products.OrderByDescending(s => propertyInfo.GetValue(s))
                    : products.OrderBy(s => propertyInfo.GetValue(s));
            }
        }

        // Pagination Section
        var totalCount = products.Count();
        var data = products.Skip(((req?.Page ?? 1) - 1) * req?.PageSize ?? 10)
            .Take(req?.PageSize ?? 10);

        return TypedResults.Ok(new PaginatedResponse<Product>
        {
            Data = data,
            Page = req?.Page ?? 1,
            PageSize = req?.PageSize ?? 10,
            TotalCount = totalCount
        });
    }
}