using System;

namespace ProductCatalogService.Domain.Dto
{
    public class FlatProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; }
        public string ImageUrl { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Guid InventoryId { get; set; }
        public string InventoryLocation { get; set; } = default!;
        public string InventoryWebsite { get; set; } = default!;
        public string InventoryDescription { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
    }
}
