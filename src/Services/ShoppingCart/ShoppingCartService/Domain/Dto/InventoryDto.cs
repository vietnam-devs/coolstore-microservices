using System;

namespace ShoppingCartService.Domain.Dto
{
    public record InventoryDto(Guid Id, string Location, string Description, string Website);
}
