using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.V1;

namespace VND.CoolStore.ProductCatalog.AppServices.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductValidator()
        {

        }
    }
}
