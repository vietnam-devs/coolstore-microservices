using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;

namespace VND.CoolStore.ProductCatalog.Usecases.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {

        }
    }
}
