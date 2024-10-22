using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Categories.Requests.Validators;

public class GetCategoryByIdRequestValidator : Validator<GetCategoryByIdRequest>
{
    public GetCategoryByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}