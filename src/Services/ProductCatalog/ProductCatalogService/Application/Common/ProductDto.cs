using System;

namespace ProductCatalogService.Application.Common
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public double Price { get; set; }
        public string ImageUrl { get; set; } = default!;
        public CategoryDto Category { get; set; } = new CategoryDto();
        public InventoryDto Inventory { get; set; } = new InventoryDto();
    }

    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class InventoryDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = default!;
        public string Website { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class SearchAggsByTagsDto
    {
        public string Key { get; set; } = default!;
        public int Count { get; set; }
    }
}
