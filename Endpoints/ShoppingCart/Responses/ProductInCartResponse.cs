using tienda_catalogo_api.Data.Models;

namespace tienda_catalogo_api.Endpoints.ShoppingCart.Responses;

public class ProductInCartResponse
{
    public required int Id { get; set; }
    public required Product Product { get; set; }
    public required int Amount { get; set; }
}