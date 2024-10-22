using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.ShoppingCart.Requests;

namespace tienda_catalogo_api.Endpoints.ShoppingCart;

public class DeleteProductsInCartBulkEndpoint(AppDbContext dbContext) : Endpoint<IEnumerable<ProductToCartRequest>, Results<Ok, UnauthorizedHttpResult,ProblemDetails>>
{
    public override void Configure()
    {
        Delete("/shopping-cart");
        AllowAnonymous();
    }
    
    public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(
        IEnumerable<ProductToCartRequest> productIds, CancellationToken ct)
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

        var products = await dbContext.ProductInCars
            .Where(p => p.SessionToken == sessionToken && productIds.Any(x => x.ProductId == p.Id))
            .ToListAsync(ct);

        dbContext.ProductInCars.RemoveRange(products);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}