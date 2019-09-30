using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;

namespace VND.CoolStore.ProductCatalog.Usecases.GetProductsByPriceAndName
{
    public class GetProductsByPriceAndNameValidator : AbstractValidator<GetProductsRequest>
    {
        public GetProductsByPriceAndNameValidator()
        {

        }
    }
}
