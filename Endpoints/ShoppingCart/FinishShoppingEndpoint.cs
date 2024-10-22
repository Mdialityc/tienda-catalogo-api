using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;

namespace tienda_catalogo_api.Endpoints.ShoppingCart;

public class FinishShoppingEndpoint(AppDbContext dbContext) : EndpointWithoutRequest<Results<Ok, UnauthorizedHttpResult, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/shopping-cart/finish");
        Roles("User");
    }
    
    public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(CancellationToken ct)
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

        var p = await dbContext.ProductInCars.Where(x => x.SessionToken == sessionToken)
            .ToListAsync(cancellationToken: ct);
        
        dbContext.ProductInCars.RemoveRange(p);
        await dbContext.SaveChangesAsync(ct);

        dbContext.SessionTokens.Remove(sessionToken);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}