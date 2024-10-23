namespace tienda_catalogo_api.Endpoints.ShoppingCart.Requests;

public class FinishShoppingCartRequest
{
    public required string Name { get; set; }
    public required string LastNames { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string? Email { get; set; }
}