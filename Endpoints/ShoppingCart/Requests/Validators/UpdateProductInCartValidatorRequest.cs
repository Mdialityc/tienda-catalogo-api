using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.ShoppingCart.Requests.Validators;

public class UpdateProductInCartValidatorRequest : Validator<UpdateProductInCartRequest>
{
    public UpdateProductInCartValidatorRequest()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}