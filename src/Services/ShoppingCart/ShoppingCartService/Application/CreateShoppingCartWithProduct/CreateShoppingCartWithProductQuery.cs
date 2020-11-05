using System;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Application.CreateShoppingCartWithProduct
{
    public class CreateShoppingCartWithProductQuery : IRequest<CartDto>
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateShoppingCartWithProductValidator : AbstractValidator<CreateShoppingCartWithProductQuery>
    {
        public CreateShoppingCartWithProductValidator()
        {
        }
    }
}
