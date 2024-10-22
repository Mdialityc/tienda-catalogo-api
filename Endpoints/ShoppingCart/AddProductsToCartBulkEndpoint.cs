using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.ShoppingCart.Requests;

namespace tienda_catalogo_api.Endpoints.ShoppingCart;

public class AddProductsToCartBulkEndpoint : Endpoint<IEnumerable<ProductToCartRequest>,
    Results<Created, UnauthorizedHttpResult, ProblemDetails>>
{
    private readonly AppDbContext _dbContext;

    public AddProductsToCartBulkEndpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Post("/shopping-cart");
        Roles("User");
    }

    public override async Task<Results<Created, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(
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

        var sessionToken = await _dbContext.SessionTokens
            .FirstOrDefaultAsync(st => st.Token == token, ct);

        if (sessionToken == null)
        {
            return TypedResults.Unauthorized();
        }
        
        sessionToken.UsedDate = DateTimeOffset.UtcNow;

        var productIdList = productIds.Select(x => x.ProductId).ToList();
        var products = await _dbContext.Products
            .Where(p => productIdList.Contains(p.Id))
            .ToListAsync(ct);

        foreach (var product in products)
        {
            var productInCar = new ProductInCart
            {
                Product = product,
                SessionToken = sessionToken,
                Amount = productIds.FirstOrDefault(x => x.ProductId == product.Id)?.Amount ?? 1
            };

            _dbContext.ProductInCars.Add(productInCar);
        }
        
        await _dbContext.SaveChangesAsync(ct);

        return TypedResults.Created();
    }
}