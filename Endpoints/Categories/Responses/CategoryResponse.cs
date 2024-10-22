namespace tienda_catalogo_api.Endpoints.Categories.Responses;

public class CategoryResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required IEnumerable<CategoryResponse>? SubCategories { get; set; }
}