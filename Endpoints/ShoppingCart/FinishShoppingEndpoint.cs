using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.ShoppingCart.Requests;
using Order = tienda_catalogo_api.Data.Models.Order;

namespace tienda_catalogo_api.Endpoints.ShoppingCart;

public class FinishShoppingEndpoint(AppDbContext dbContext) : Endpoint<FinishShoppingCartRequest, Results<Ok, UnauthorizedHttpResult, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/shopping-cart/finish");
        Roles("User");
    }
    
    public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(FinishShoppingCartRequest req, CancellationToken ct)
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return TypedResults.Unauthorized();
        }

        var token = authorizationHeader.StartsWith("Bearer ")
            ? authorizationHeader.Substring("Bearer ".Length).Trim()
            : authorizationHeader;

        var sessionToken = await dbContext.SessionTokens
            .FirstOrDefaultAsync(st => st.Token == token, ct);

        if (sessionToken == null)
        {
            return TypedResults.Unauthorized();
        }

        if (req.PhoneNumber is null && req.Email is null)
        {
            AddError("Order must have a phone number or an email");
        }

        var p = await dbContext.ProductInCars.Where(x => x.SessionToken == sessionToken)
            .Include(productInCart => productInCart.Product)
            .ToListAsync(cancellationToken: ct);
        
        if (!p.Any())
        {
            AddError("Order can not exist without any products.");
        }
        
        ThrowIfAnyErrors();

        var products = p.Select(x => x.Product).ToList();

        await  dbContext.Orders.AddAsync(new Order
        {
            Email = req.Email,
            Name = req.Name,
            Products = products,
            Status = OrderStatus.Pending,
            CreatedDate = DateTimeOffset.UtcNow,
            LastNames = req.LastNames,
            PhoneNumber = req.PhoneNumber
        }, ct);
        dbContext.ProductInCars.RemoveRange(p);
        dbContext.SessionTokens.Remove(sessionToken);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}