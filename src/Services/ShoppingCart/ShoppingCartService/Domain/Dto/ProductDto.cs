using System;

namespace ShoppingCartService.Domain.Dto
{
    public record ProductDto(Guid Id, string Name, double Price, string ImageUrl, string Description,
        Guid? InventoryId, string? InventoryLocation, string? InventoryWebsite, string? InventoryDescription,
        Guid CategoryId, string CategoryName);
}
