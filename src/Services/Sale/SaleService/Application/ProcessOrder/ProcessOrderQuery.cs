using FluentValidation;
using MediatR;

namespace SaleService.Application.ProcessOrder
{
    public class ProcessOrderQuery : IRequest<bool>
    {
    }

    public class ProcessOrderValidator : AbstractValidator<ProcessOrderQuery>
    {
        public ProcessOrderValidator()
        {
        }
    }
}
