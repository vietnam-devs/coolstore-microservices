using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace InventoryService.Application.GetAvailabilityInventories
{
    public class GetAvailabilityInventoriesQuery : IRequest<IEnumerable<InventoryDto>>
    {
        public IEnumerable<Guid> Ids { get; set; } = new List<Guid>();
    }

    public class GetAvailabilityInventoriesQueryValidator : AbstractValidator<GetAvailabilityInventoriesQuery>
    {
    }
}
