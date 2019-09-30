using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;

namespace VND.CoolStore.ShoppingCart.Usecases.DeleteProductInShoppingCart
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
