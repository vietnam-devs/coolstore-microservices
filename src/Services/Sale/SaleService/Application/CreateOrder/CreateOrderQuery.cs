using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace SaleService.Application.CreateOrder
{
    public class CreateOrderQuery : IRequest<bool>
    {
        public CartDto Cart { get; set; } = default!;
    }

    public class CreateOrderValidator : AbstractValidator<CreateOrderQuery>
    {
        public CreateOrderValidator()
        {
        }
    }
}
