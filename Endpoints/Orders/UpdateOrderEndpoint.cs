using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Orders.Requests;

namespace tienda_catalogo_api.Endpoints.Orders;

public class UpdateOrderEndpoint(AppDbContext dbContext) : Endpoint<UpdateOrderRequest, Results<Ok, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Put("/orders/{id}");
        Roles("Admin");
    }
    
    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(UpdateOrderRequest req, CancellationToken ct)
    {
        var order = await dbContext.Orders.FindAsync(req.Id, ct);

        if (order is null)
            return TypedResults.NotFound();

        if (req.PhoneNumber is null && req.Email is null)
        {
            AddError("Order must have a phone number or an email");
        }
        
        ThrowIfAnyErrors();
        
        order.Name = req.Name;
        order.LastNames = req.LastNames;
        order.PhoneNumber = req.PhoneNumber;
        order.Email = req.Email;
        order.Status = req.Status;
        
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}