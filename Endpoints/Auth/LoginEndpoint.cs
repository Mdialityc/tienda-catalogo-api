using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Auth.Requests;
using tienda_catalogo_api.Endpoints.Auth.Responses;
using tienda_catalogo_api.Utils.Options;
using tienda_catalogo_api.Utils.Tokens;

namespace tienda_catalogo_api.Endpoints.Auth;

public class LoginEndpoint
    : Endpoint<LoginRequest, Results<Ok<LoginResponse>, UnauthorizedHttpResult, ProblemDetails>>
{
    public IOptions<AuthOptions> AuthOptions { get; set; }

    public override void Configure()
    {
        Post("auth/login");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<LoginResponse>, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(
        LoginRequest req, CancellationToken ct)
    {
        if (AuthOptions.Value.AdminUsername != req.Username || AuthOptions.Value.AdminPassword != req.Password)
        {
            Console.WriteLine(1);
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok(new LoginResponse
        {
            Token = TokenGenerator.GenerateJwtToken(AuthOptions.Value.AdminUsername, AuthOptions.Value.AdminPassword,
                AuthOptions.Value.Key)
        });
    }
}