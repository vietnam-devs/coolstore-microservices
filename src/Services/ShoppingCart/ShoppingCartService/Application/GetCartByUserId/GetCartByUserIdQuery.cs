using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Application.GetCartByUserId
{
    public class GetCartByUserIdQuery : IRequest<CartDto>
    {
    }

    public class GetShoppingCartWithProductsValidator : AbstractValidator<GetCartByUserIdQuery>
    {
        public GetShoppingCartWithProductsValidator()
        {
        }
    }
}
