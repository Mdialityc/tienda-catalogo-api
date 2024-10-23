using tienda_catalogo_api.Data.Models;

namespace tienda_catalogo_api.Endpoints.Orders.Requests;

public class GetOrdersRequest
{
    public int[]? Ids { get; set; }
    public int[]? ProductIds { get; set; }
    public string? Name { get; set; }
    public string? LastNames { get; set; }
    public OrderStatus? Status { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? MinimumCreatedDate { get; set; }
    public DateTimeOffset? MaximumCreatedDate { get; set; }
    public string? SortBy { get; set; } = "Name";
    public bool? IsDescending { get; set; } = false;
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}