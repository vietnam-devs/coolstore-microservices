using System;
using FluentValidation;
using MediatR;
using SaleService.Domain.Model;

namespace SaleService.Application.UpdateOrderStatus
{
    public class UpdateOrderStatusQuery : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    internal class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusQuery>
    {
        public UpdateOrderStatusValidator()
        {
        }
    }
}
