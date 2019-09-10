using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.AppService.DeleteProductInShoppingCart
{
    public class DeleteProductInShoppingCartValidator : AbstractValidator<DeleteItemRequest>
    {
        public DeleteProductInShoppingCartValidator()
        {
            RuleFor(x => x.CartId)
                .NotNull()
                .NotEmpty()
                .WithMessage("The cart id could not be null or empty.");
        }
    }
}
