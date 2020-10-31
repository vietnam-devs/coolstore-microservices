using System;

namespace InventoryService.Application.Common
{
    public record InventoryDto(Guid Id, string Location, string Description, string Website);
}
