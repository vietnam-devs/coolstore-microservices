using FluentValidation;
using VND.CoolStore.ShoppingCart.DataContracts.Api.V1;

namespace VND.CoolStore.ShoppingCart.Usecases.GetCartByUserId
{
    public class GetShoppingCartWithProductsValidator : AbstractValidator<GetCartByUserIdRequest>
    {
        public GetShoppingCartWithProductsValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage("The user id could not be null or empty.");
        }
    }
}
