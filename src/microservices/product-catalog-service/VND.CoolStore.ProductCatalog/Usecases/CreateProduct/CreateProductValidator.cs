using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;

namespace VND.CoolStore.ProductCatalog.Usecases.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductValidator()
        {

        }
    }
}
