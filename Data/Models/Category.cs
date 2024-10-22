namespace tienda_catalogo_api.Data.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public int? ParentCategoryId { get; set; }
    
    public Category? ParentCategory { get; set; }
    
    public ICollection<Category> Subcategories { get; set; } = new List<Category>();
}