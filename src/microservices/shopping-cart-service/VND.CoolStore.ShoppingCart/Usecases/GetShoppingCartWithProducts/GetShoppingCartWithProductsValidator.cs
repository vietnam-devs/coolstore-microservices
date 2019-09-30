using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;

namespace VND.CoolStore.ShoppingCart.Usecases.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsValidator : AbstractValidator<GetCartRequest>
    {
        public GetShoppingCartWithProductsValidator()
        {
            RuleFor(x => x.CartId)
                .NotNull()
                .NotEmpty()
                .WithMessage("The cart id could not be null or empty.");
        }
    }
}
