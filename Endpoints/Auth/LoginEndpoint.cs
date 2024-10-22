using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using tienda_catalogo_api.Endpoints.Auth.Requests;
using tienda_catalogo_api.Utils.Options;
using tienda_catalogo_api.Utils.Tokens;

namespace tienda_catalogo_api.Endpoints.Auth;

public class LoginEndpoint : Endpoint<LoginRequest, Results<Ok<string>, UnauthorizedHttpResult, ProblemDetails>>
{
    private readonly AuthOptions _authOptions;

    public LoginEndpoint(IOptions<AuthOptions> authOptions)
    {
        this._authOptions = authOptions.Value;
    }

    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<string>, UnauthorizedHttpResult, ProblemDetails>> HandleAsync(
        LoginRequest req, CancellationToken ct)
    {
        if (_authOptions.AdminUsername != req.Username || _authOptions.AdminPassword != req.Password)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(TokenGenerator.GenerateJwtToken(_authOptions.AdminUsername, _authOptions.AdminPassword,
            _authOptions.Key));
    }
}