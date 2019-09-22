using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.V1;

namespace VND.CoolStore.ShoppingCart.Usecases.CheckOutShoppingCart
{
    public class CheckOutShoppingCartValidator : AbstractValidator<CheckoutRequest>
    {
        public CheckOutShoppingCartValidator()
        {
            RuleFor(x => x.CartId)
                .NotNull()
                .NotEmpty()
                .WithMessage("The cart id could not be null or empty.");
        }
    }
}
