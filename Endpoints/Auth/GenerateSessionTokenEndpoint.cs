using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using tienda_catalogo_api.Data;
using tienda_catalogo_api.Data.Models;
using tienda_catalogo_api.Endpoints.Auth.Responses;
using tienda_catalogo_api.Utils.Tokens;

namespace tienda_catalogo_api.Endpoints.Auth;

public class GenerateSessionTokenEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<Results<Ok<GenerateSessionTokenResponse>, ProblemDetails>>
{
    public override void Configure()
    {
        Get("/auth/generate-token");
        AllowAnonymous();
    }

    public override async Task<Results<Ok<GenerateSessionTokenResponse>, ProblemDetails>> ExecuteAsync(CancellationToken ct)
    {
        var st = TokenGenerator.GenerateSessionToken();
        await dbContext.SessionTokens.AddAsync(new SessionToken
        {
            Token = st,
            UsedDate = DateTimeOffset.UtcNow
        }, cancellationToken: ct);
        await dbContext.SaveChangesAsync(ct);
        return TypedResults.Ok(new GenerateSessionTokenResponse{SessionToken = st});
    }
}