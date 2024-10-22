using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace tienda_catalogo_api.Utils.Tokens;

public class TokenGenerator
{
    public static string GenerateJwtToken(string username, string password, string secretKey)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "Admin"),
            new Claim(ClaimTypes.Name, username),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public static string GenerateSessionToken() => Guid.NewGuid().ToString("N");
}