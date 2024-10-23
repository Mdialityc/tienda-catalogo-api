using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Orders.Requests;

namespace tienda_catalogo_api.Endpoints.Orders;

public class DeleteOrderEndpoint(AppDbContext dbContext) : Endpoint<GetOrderByIdRequest, Results<Ok, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Delete("/orders/{id}");
        Roles("Admin");
    }
    
    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(GetOrderByIdRequest req, CancellationToken ct)
    {
        var o = await dbContext.Orders.FindAsync(req.Id, ct);

        if (o is null)
            return TypedResults.NotFound();

        dbContext.Orders.Remove(o);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}