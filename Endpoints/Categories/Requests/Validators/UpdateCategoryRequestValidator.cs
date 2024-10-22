using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Categories.Requests.Validators;

public class UpdateCategoryRequestValidator: Validator<UpdateCategoryRequest>
{
    public UpdateCategoryRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Image)
            .NotEmpty();

        RuleFor(x => x.ParentId)
            .GreaterThan(0);
    }
}