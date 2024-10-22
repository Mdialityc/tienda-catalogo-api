namespace tienda_catalogo_api.Data.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required string? Link { get; set; }
    public required string Specifications { get; set; }
    public required string Description { get; set; }
    public required int CategoryId { get; set; }
    public required bool HasStock { get; set; }
    public required decimal Price { get; set; }
    public required string Currency { get; set; }
    public required bool HasSale { get; set; } // Oferta
    public required DateTimeOffset? SaleStart { get; set; }
    public required DateTimeOffset? SaleEnd { get; set; }
    public required string? SaleDescription { get; set; }
    public required decimal? DiscountAmount { get; set; }
}