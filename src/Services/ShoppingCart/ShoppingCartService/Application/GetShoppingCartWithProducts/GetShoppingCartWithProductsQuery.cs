using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Application.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsQuery : IRequest<IEnumerable<FlatCartDto>>
    {
        public Guid UserId { get; set; }
    }

    public class GetShoppingCartWithProductsValidator : AbstractValidator<GetShoppingCartWithProductsQuery>
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
