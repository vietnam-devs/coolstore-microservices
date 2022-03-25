using ShoppingCart.Core.Dtos;

namespace ShoppingCart.UseCases.GetCartByUserId;

public class GetCartByUserIdQuery : IRequest<CartDto>
{
}

public class GetShoppingCartWithProductsValidator : AbstractValidator<GetCartByUserIdQuery>
{
    public GetShoppingCartWithProductsValidator()
    {
    }
}
