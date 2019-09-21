using FluentValidation;

namespace VND.CoolStore.ProductCatalog.Usecases.DeleteProduct
{
    using VND.CoolStore.ProductCatalog.DataContracts.V1;

    public class DeleteProductValidator : AbstractValidator<DeleteProductRequest>
    {
        public DeleteProductValidator()
        {

        }
    }
}
