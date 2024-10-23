using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Orders.Requests.Validators;

public class UpdateOrderRequestValidator : Validator<UpdateOrderRequest>
{
    public UpdateOrderRequestValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.LastNames)
            .NotEmpty();

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}