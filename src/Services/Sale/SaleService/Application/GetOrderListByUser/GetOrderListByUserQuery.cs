using System.Collections.Generic;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace SaleService.Application.GetOrderListByUser
{
    public class GetOrderListByUserQuery : IRequest<IEnumerable<OrderDto>>
    {
    }

    public class GetOrderListByUserValidator : AbstractValidator<GetOrderListByUserQuery>
    {
        public GetOrderListByUserValidator()
        {
        }
    }
}
