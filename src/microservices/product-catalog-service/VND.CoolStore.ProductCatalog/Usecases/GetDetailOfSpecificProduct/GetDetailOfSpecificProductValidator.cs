using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;

namespace VND.CoolStore.ProductCatalog.Usecases.GetDetailOfSpecificProduct
{
    public class GetDetailOfSpecificProductValidator : AbstractValidator<GetProductByIdRequest>
    {
        public GetDetailOfSpecificProductValidator()
        {

        }
    }
}
