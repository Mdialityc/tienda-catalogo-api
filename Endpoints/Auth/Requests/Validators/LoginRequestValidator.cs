using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Auth.Requests.Validators;

public class LoginRequestValidator: Validator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}