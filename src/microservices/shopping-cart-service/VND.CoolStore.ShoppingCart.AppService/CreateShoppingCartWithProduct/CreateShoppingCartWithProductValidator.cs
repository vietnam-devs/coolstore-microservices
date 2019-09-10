using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.CreateShoppingCartWithProduct
{
    public class CreateShoppingCartWithProductValidator : AbstractValidator<InsertItemToNewCartRequest>
    {
        public CreateShoppingCartWithProductValidator()
        {
        }
    }
}
