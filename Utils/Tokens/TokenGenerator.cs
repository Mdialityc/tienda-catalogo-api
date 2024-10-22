using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastEndpoints.Security;
using Microsoft.IdentityModel.Tokens;

namespace tienda_catalogo_api.Utils.Tokens;

public class TokenGenerator
{
    public static string GenerateJwtToken(string username, string secretKey)
    {
        return JwtBearer.CreateToken(o =>
        {
            o.ExpireAt = DateTime.UtcNow.AddDays(2);
            o.SigningKey = secretKey;
            o.User.Roles.Add("Admin");
            o.User.Claims.Add(("UserName", username));
        });;
    }
    
    public static string GenerateSessionToken(string secretKey)
    {
        return JwtBearer.CreateToken(o =>
        {
            o.ExpireAt = DateTime.UtcNow.AddMonths(2);
            o.SigningKey = secretKey;
            o.User.Roles.Add("User");
            o.User.Claims.Add(("UserName", "User"));
        });;
    }
}