namespace tienda_catalogo_api.Endpoints.Categories.Requests;

public class UpdateCategoryRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required int? ParentId { get; set; }
}