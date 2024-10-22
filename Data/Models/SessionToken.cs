namespace tienda_catalogo_api.Data.Models;

public class SessionToken
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public required DateTimeOffset UsedDate { get; set; }
}