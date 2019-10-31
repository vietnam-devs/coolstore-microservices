using FluentValidation;
using VND.CoolStore.Search.DataContracts.Api.V1;

namespace VND.CoolStore.Search.Usecases.SearchProduct
{
    public class SearchProductValidator : AbstractValidator<SearchProductRequest>
    {
        public SearchProductValidator()
        {
        }
    }
}
