using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Products.Requests;

namespace tienda_catalogo_api.Endpoints.Products;

public class CreateProductEndpoint(AppDbContext dbContext) : Endpoint<CreateProductRequest, Results<Created<Product>, Conflict, ProblemDetails>>
{
    public override void Configure()
    {
        Post("/products");
        Roles("Admin");
    }
    
    public override async Task<Results<Created<Product>, Conflict, ProblemDetails>> ExecuteAsync(CreateProductRequest req, CancellationToken ct)
    {
        var product = new Product
        {
            Currency = req.Currency,
            Name = req.Name.Trim(),
            Description = req.Description,
            Image = req.Image,
            Link = req.Link,
            Price = req.Price,
            Specifications = req.Specifications,
            CategoryId = req.CategoryId,
            DiscountAmount = req.DiscountAmount,
            HasSale = req.HasSale,
            HasStock = req.HasStock,
            SaleDescription = req.SaleDescription,
            SaleEnd = req.SaleEnd?.ToUniversalTime(),
            SaleStart = req.SaleStart?.ToUniversalTime()
        };

        if (dbContext.Products.AsNoTracking().Any(x => x.Name == product.Name))
        {
            return TypedResults.Conflict();
        }
            

        await dbContext.Products.AddAsync(product, ct);
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Created($"/products/{product.Id}", product);
    }
}