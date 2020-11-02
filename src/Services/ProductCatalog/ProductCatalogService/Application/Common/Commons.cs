using System;

namespace ProductCatalogService.Application.Common
{
    // public record ProductDto(Guid Id, string Name, double Price, string ImageUrl, string Description,
    //     Guid? InventoryId, string? InventoryLocation, string? InventoryWebsite, string? InventoryDescription,
    //     Guid CategoryId, string CategoryName);
    // public record InventoryDto(Guid Id, string Location, string Description, string Website);
    public record SearchAggsByTagsDto(string Key, int Count);
}
