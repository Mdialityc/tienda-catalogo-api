using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Commons.Responses;
using tienda_catalogo_api.Endpoints.Orders.Requests;
using Order = tienda_catalogo_api.Data.Models.Order;

namespace tienda_catalogo_api.Endpoints.Orders;

public class GetOrdersEndpoint(AppDbContext dbContext) : Endpoint<GetOrdersRequest, Results<Ok<PaginatedResponse<Order>>, ProblemDetails>>
{
    public override void Configure()
    {
        Get("/orders");
        Roles("Admin");
    }
    
    public override async Task<Results<Ok<PaginatedResponse<Order>>, ProblemDetails>>
        ExecuteAsync(GetOrdersRequest req, CancellationToken ct)
    {
        var query = dbContext.Orders
            .Include(x => x.Products)
            .AsNoTracking().AsQueryable();

        // Filtering Section
        if (req.Ids?.Any() ?? false)
        {
            query = query.Where(x => req.Ids.Contains(x.Id));
        }
        
        if (req.ProductIds?.Any() ?? false)
        {
            query = query.Where(x => x.Products.Any(x => req.ProductIds.Contains(x.Id)));
        }

        if (req.Name is not null)
        {
            query = query.Where(x => x.Name.Contains(req.Name.Trim()));
        }
        
        if (req.LastNames is not null)
        {
            query = query.Where(x => x.LastNames.Contains(req.LastNames.Trim()));
        }

        if (req.Status is not null)
        {
            query = query.Where(x => req.Status == x.Status);
        }

        if (req.PhoneNumber is not null)
        {
            query = query.Where(x => req.PhoneNumber == x.PhoneNumber);
        }

        if (req.Email is not null)
        {
            query = query.Where(x => req.Email == x.Email);
        }

        if (req.MinimumCreatedDate is not null)
        {
            req.MinimumCreatedDate = req.MinimumCreatedDate.Value.ToUniversalTime();
            query = query.Where(x => x.CreatedDate >= req.MinimumCreatedDate);
        }
        
        if (req.MaximumCreatedDate is not null)
        {
            req.MaximumCreatedDate = req.MaximumCreatedDate.Value.ToUniversalTime();
            query = query.Where(x => x.CreatedDate <= req.MaximumCreatedDate);
        }
        
        var orders = (await query.ToListAsync(ct)).AsEnumerable();

        // Sorting Section
        if (!string.IsNullOrEmpty(req.SortBy))
        {
            var propertyInfo = typeof(Order).GetProperty(req.SortBy);
            if (propertyInfo != null)
            {
                orders = req?.IsDescending ?? false
                    ? orders.OrderByDescending(s => propertyInfo.GetValue(s))
                    : orders.OrderBy(s => propertyInfo.GetValue(s));
            }
        }

        // Pagination Section
        var totalCount = orders.Count();
        var data = orders.Skip(((req?.Page ?? 1) - 1) * req?.PageSize ?? 10)
            .Take(req?.PageSize ?? 10);

        return TypedResults.Ok(new PaginatedResponse<Order>
        {
            Data = data,
            Page = req?.Page ?? 1,
            PageSize = req?.PageSize ?? 10,
            TotalCount = totalCount
        });
    }
}