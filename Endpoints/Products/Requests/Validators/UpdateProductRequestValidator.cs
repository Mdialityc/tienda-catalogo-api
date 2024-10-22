using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Products.Requests.Validators;

public class UpdateProductRequestValidator : Validator<UpdateProductRequest>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
        
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Image)
            .NotEmpty();

        RuleFor(x => x.Specifications)
            .NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Currency)
            .NotEmpty();

        RuleFor(x => x.DiscountAmount)
            .GreaterThanOrEqualTo(0);
    }
}