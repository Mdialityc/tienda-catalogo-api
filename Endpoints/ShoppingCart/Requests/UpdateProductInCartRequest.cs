namespace tienda_catalogo_api.Endpoints.ShoppingCart.Requests;

public class UpdateProductInCartRequest
{
    public required int Id { get; set; }
    public required int Amount { get; set; }
}