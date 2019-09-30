using FluentValidation;
using VND.CoolStore.ProductCatalog.DataContracts.Api.V1;

namespace VND.CoolStore.ProductCatalog.Usecases.DeleteProduct
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductValidator()
        {
        }
    }
}
