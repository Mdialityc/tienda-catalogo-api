namespace tienda_catalogo_api.Endpoints.Auth.Requests;

public class ValidateSessionTokenRequest
{
    public required string SessionToken { get; set; }
}