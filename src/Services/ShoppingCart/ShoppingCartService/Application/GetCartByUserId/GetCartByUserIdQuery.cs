using System;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Application.GetCartByUserId
{
    public class GetCartByUserIdQuery : IRequest<CartDto>
    {
        public Guid UserId { get; set; }
    }

    public class GetShoppingCartWithProductsValidator : AbstractValidator<GetCartByUserIdQuery>
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
