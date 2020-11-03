using System;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace InventoryService.Application.GetInventory
{
    public class GetInventoryQuery : IRequest<InventoryDto>
    {
        public Guid Id { get; set; }
    }

    public class GetInventoryValidator : AbstractValidator<GetInventoryQuery>
    {
        public GetInventoryValidator()
        {
        }
    }
}
