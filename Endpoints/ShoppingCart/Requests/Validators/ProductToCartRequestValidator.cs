using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.ShoppingCart.Requests.Validators;

public class ProductToCartRequestValidator : Validator<ProductToCartRequest>
{
    public ProductToCartRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);
        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}