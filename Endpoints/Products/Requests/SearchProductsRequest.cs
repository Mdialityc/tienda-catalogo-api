namespace tienda_catalogo_api.Endpoints.Products.Requests;

public class SearchProductsRequest
{
    public int[]? Ids { get; set; }
    public string[]? Names { get; set; }
    public int[]? CategoryIds { get; set; }
    public bool? HasStock { get; set; }
    public string? Currency { get; set; }
    public decimal? MinimumPrice { get; set; }
    public decimal? MaximumPrice { get; set; }
    public bool? HasSale { get; set; }
    public DateTimeOffset? SalesStart { get; set; }
    public DateTimeOffset? SalesEnd { get; set; }
    public bool? HasDiscount { get; set; }
    public decimal? MinimumDiscountAmount { get; set; }
    public decimal? MaximumDiscountAmount { get; set; }
    public string? SortBy { get; set; } = "Name";
    public bool? IsDescending { get; set; } = false;
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
}