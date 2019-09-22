using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.Usecases.UpdateAmountOfProductInShoppingCart
{
    public class UpdateAmountOfProductInShoppingCartValidator : AbstractValidator<UpdateItemInCartRequest>
    {
        public UpdateAmountOfProductInShoppingCartValidator()
        {
        }
    }
}
