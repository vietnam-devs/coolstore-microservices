using System;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Application.UpdateAmountOfProductInShoppingCart
{
    public class UpdateAmountOfProductInShoppingCartQuery : IRequest<CartDto>
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateAmountOfProductInShoppingCartValidator : AbstractValidator<UpdateAmountOfProductInShoppingCartQuery>
    {
        public UpdateAmountOfProductInShoppingCartValidator()
        {
        }
    }
}
