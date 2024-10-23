using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Orders.Requests.Validators;

public class GetOrderByIdRequestValidator : Validator<GetOrderByIdRequest>
{
    public GetOrderByIdRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);
    }
}