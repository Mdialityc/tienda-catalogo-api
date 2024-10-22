using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Auth.Requests.Validators;

public class ValidateSessionTokenRequestValidator : Validator<ValidateSessionTokenRequest>
{
    public ValidateSessionTokenRequestValidator()
    {
        RuleFor(x => x.SessionToken)
            .NotEmpty();
    }
}