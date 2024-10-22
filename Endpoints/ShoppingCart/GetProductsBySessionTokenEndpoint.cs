using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;

namespace tienda_catalogo_api.Endpoints.ShoppingCart;

public class GetProductsBySessionTokenEndpoint : EndpointWithoutRequest<
    Results<Ok<IEnumerable<Product>>, UnauthorizedHttpResult, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public GetProductsBySessionTokenEndpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/shopping-cart");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<IEnumerable<Product>>, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(
        CancellationToken ct)
    {
        var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return TypedResults.Unauthorized();
        }

        var token = authorizationHeader.StartsWith("Bearer ")
            ? authorizationHeader.Substring("Bearer ".Length).Trim()
            : authorizationHeader;

        var sessionToken = await _dbContext.SessionTokens
            .FirstOrDefaultAsync(st => st.Token == token, ct);

        if (sessionToken == null)
        {
            return TypedResults.Unauthorized();
        }
        
        sessionToken.UsedDate = DateTimeOffset.UtcNow;

        var productsInCar = await _dbContext.ProductInCars
            .Include(p => p.Product)
            .Where(pic => pic.SessionToken.Id == sessionToken.Id)
            .ToListAsync(ct);

        var products = productsInCar.Select(pic => pic.Product).ToList().AsEnumerable();

        return TypedResults.Ok(products);
    }
}