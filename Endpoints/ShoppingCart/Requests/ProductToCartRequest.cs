namespace tienda_catalogo_api.Endpoints.ShoppingCart.Requests;

public class ProductToCartRequest
{
    public required int ProductId { get; set; }
    public required int Amount { get; set; }
}