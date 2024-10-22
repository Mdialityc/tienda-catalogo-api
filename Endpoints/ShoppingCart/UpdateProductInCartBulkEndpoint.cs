using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.ShoppingCart.Requests;

namespace tienda_catalogo_api.Endpoints.ShoppingCart;

public class UpdateProductInCartBulkEndpoint(AppDbContext dbContext) : Endpoint<IEnumerable<UpdateProductInCartRequest>, Results<Ok, UnauthorizedHttpResult, ProblemDetails>>
{
    public override void Configure()
    {
        Put("/shopping-cart");
        Roles("User");
    }
    
    public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(
        IEnumerable<UpdateProductInCartRequest> productIds, CancellationToken ct)
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

        sessionToken.UsedDate = DateTimeOffset.UtcNow;

        foreach (var p in productIds)
        {
            var pc = dbContext.ProductInCars.FirstOrDefault(x => x.Id == p.Id);
            if (pc is not null)
            {
                pc.Amount = p.Amount;
            }
        }
        
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}