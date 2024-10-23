using tienda_catalogo_api.Data.Models;

namespace tienda_catalogo_api.Endpoints.Orders.Requests;

public class UpdateOrderRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string LastNames { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string? Email { get; set; }
    public required OrderStatus Status { get; set; }
}