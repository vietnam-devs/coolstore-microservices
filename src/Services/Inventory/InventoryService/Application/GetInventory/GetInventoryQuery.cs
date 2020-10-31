using System;
using FluentValidation;
using InventoryService.Application.Common;
using MediatR;

namespace InventoryService.Application.GetInventory
{
    public class GetInventoryQuery : IRequest<InventoryDto>
    {
        public Guid Id { get; set; }
    }

    public record InventoryRequest(Guid InventoryId);

    public class GetInventoryValidator : AbstractValidator<GetInventoryQuery>
    {
        public GetInventoryValidator()
        {
        }
    }
}
