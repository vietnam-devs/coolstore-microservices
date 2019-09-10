using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.UpdateAmountOfProductInShoppingCart
{
    public class UpdateAmountOfProductInShoppingCartValidator : AbstractValidator<UpdateItemInCartRequest>
    {
        public UpdateAmountOfProductInShoppingCartValidator()
        {
        }
    }
}
