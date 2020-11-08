using FluentValidation;
using MediatR;

namespace SaleService.Application.CompleteOrder
{
    public class CompleteOrderQuery : IRequest<bool>
    {
    }

    public class CompleteOrderValidator : AbstractValidator<CompleteOrderQuery>
    {
        public CompleteOrderValidator()
        {
        }
    }
}
