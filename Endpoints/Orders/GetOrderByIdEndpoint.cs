using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Orders.Requests;
using Order = tienda_catalogo_api.Data.Models.Order;

namespace tienda_catalogo_api.Endpoints.Orders;

public class GetOrderByIdEndpoint(AppDbContext dbContext) : Endpoint<GetOrderByIdRequest, Results<Ok<Order>, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Get("/orders/{id}");
        Roles("Admin");
    }
    
    public override async Task<Results<Ok<Order>, NotFound, ProblemDetails>> ExecuteAsync(GetOrderByIdRequest req, CancellationToken ct)
    {
        var o = await dbContext.Orders
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (o is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(o);
    }
}