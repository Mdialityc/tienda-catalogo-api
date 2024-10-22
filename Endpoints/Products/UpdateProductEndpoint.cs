using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Products.Requests;

namespace tienda_catalogo_api.Endpoints.Products;

public class UpdateProductEndpoint(AppDbContext dbContext) : Endpoint<UpdateProductRequest, Results<Ok, NotFound, ProblemDetails>>
{
    public override void Configure()
    {
        Put("/products/{id}");
        Roles("Admin");
    }
    
    public override async Task<Results<Ok, NotFound, ProblemDetails>> ExecuteAsync(UpdateProductRequest req, CancellationToken ct)
    {
        var product = await dbContext.Products.FindAsync(req.Id, ct);

        if (product is null)
            return TypedResults.NotFound();

        product.Name = req.Name;
        product.Image = req.Image;
        product.Link = req.Link;
        product.Specifications = req.Specifications;
        product.Description = req.Description;
        product.CategoryId = req.CategoryId;
        product.HasStock = req.HasStock;
        product.Price = req.Price;
        product.Currency = req.Currency;
        product.HasSale = req.HasSale;
        product.SaleStart = req.SaleStart;
        product.SaleEnd = req.SaleEnd;
        product.SaleDescription = req.SaleDescription;
        product.DiscountAmount = req.DiscountAmount;
        
        await dbContext.SaveChangesAsync(ct);

        return TypedResults.Ok();
    }
}