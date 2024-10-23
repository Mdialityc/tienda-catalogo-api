using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Orders.Requests.Validators;

public class GetOrdersRequestValidator: Validator<GetOrdersRequest>
{
    public GetOrdersRequestValidator()
    {
        RuleFor(x => x.PageSize)
            .GreaterThan(0);
        RuleFor(x => x.Page)
            .GreaterThan(0);
    }
}