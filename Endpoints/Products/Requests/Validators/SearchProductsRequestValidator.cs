using FastEndpoints;
using FluentValidation;

namespace tienda_catalogo_api.Endpoints.Products.Requests.Validators;

public class SearchProductsRequestValidator : Validator<SearchProductsRequest>
{
    public SearchProductsRequestValidator()
    {
        RuleFor(e => e.Page)
            .GreaterThan(0);

        RuleFor(e => e.PageSize)
            .GreaterThan(0);
    }
}