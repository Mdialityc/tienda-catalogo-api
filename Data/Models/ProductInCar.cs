namespace tienda_catalogo_api.Data.Models;

public class ProductInCar
{
    public int Id { get; set; }
    public Product Product { get; set; }
    public SessionToken SessionToken { get; set; } 
    public required int Amount { get; set; }
}