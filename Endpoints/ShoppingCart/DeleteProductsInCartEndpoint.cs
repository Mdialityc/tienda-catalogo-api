using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.ShoppingCart.Requests;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace tienda_catalogo_api.Endpoints.ShoppingCart;

public class DeleteProductsInCartEndpoint(AppDbContext dbContext) : Endpoint<DeleteProductCartRequest,Results<Ok, UnauthorizedHttpResult, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Delete("/shopping-cart/{id}");
        Roles("User");
    }
    
    public override async Task<Results<Ok, UnauthorizedHttpResult, NotFound, ProblemDetails>> ExecuteAsync(
        DeleteProductCartRequest req, CancellationToken ct)
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

        var product = dbContext.ProductInCars
            .FirstOrDefault(p => p.SessionToken == sessionToken && req.Id == p.Id);

        if (product is null)
        {
            return TypedResults.NotFound();
        }
        
        dbContext.ProductInCars.Remove(product);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}