using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Application.Checkout
{
    public class CheckOutQuery : IRequest<CartDto>
    {
    }

    public class CheckOutValidator : AbstractValidator<CheckOutQuery>
    {
    }
}
