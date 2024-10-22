using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Endpoints.Auth.Requests;

namespace tienda_catalogo_api.Endpoints.Auth;

public class ValidateSessionTokenEndpoint(AppDbContext dbContext)
    : Endpoint<ValidateSessionTokenRequest, Results<Ok, UnauthorizedHttpResult, ProblemDetails>>
{
    public override void Configure()
    {
        Post("auth/validate-token");
        AllowAnonymous();
    }

    public override async Task<Results<Ok, UnauthorizedHttpResult, ProblemDetails>> ExecuteAsync(
        ValidateSessionTokenRequest req, CancellationToken ct)
    {
        if (dbContext.SessionTokens.AsNoTracking()
                .FirstOrDefault(x => x.Token == req.SessionToken) is null)
        {
            return TypedResults.Unauthorized();
        }

        return TypedResults.Ok();
    }
}