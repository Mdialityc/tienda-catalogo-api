using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.ShoppingCart.Requests.Validators;

public class DeleteProductCartRequestValidator : Validator<DeleteProductCartRequest>
{
    public DeleteProductCartRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}