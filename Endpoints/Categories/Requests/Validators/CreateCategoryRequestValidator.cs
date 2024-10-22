using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Categories.Requests.Validators;

public class CreateCategoryRequestValidator : Validator<CreateCategoryRequest>
{
    public CreateCategoryRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Image)
            .NotEmpty();

        RuleFor(x => x.ParentId)
            .GreaterThan(0);
    }
}