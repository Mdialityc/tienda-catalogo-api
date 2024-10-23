using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.ShoppingCart.Requests.Validators;

public class FinishShoppingCartRequestValidator : Validator<FinishShoppingCartRequest>
{
    public FinishShoppingCartRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.LastNames)
            .NotEmpty();
    }
}