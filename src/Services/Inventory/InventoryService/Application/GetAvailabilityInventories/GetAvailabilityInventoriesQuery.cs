using System;
using System.Collections.Generic;
using FluentValidation;
using MediatR;

namespace InventoryService.Application.GetAvailabilityInventories
{
    public class GetAvailabilityInventoriesQuery : IRequest<IEnumerable<Common.InventoryDto>>
    {
        public IEnumerable<Guid> Ids { get; set; } = new List<Guid>();
    }

    public record InventoryByIdsRequest(IEnumerable<Guid> InventoryIds);

    public class GetAvailabilityInventoriesQueryValidator : AbstractValidator<GetAvailabilityInventoriesQuery>
    {
    }
}
