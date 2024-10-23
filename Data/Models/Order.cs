namespace tienda_catalogo_api.Data.Models;

public class Order
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string LastNames { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string? Email { get; set; }
    public required DateTimeOffset CreatedDate { get; set; }
    public required OrderStatus Status { get; set; }

    public required List<Product> Products { get; set; } = new();
}