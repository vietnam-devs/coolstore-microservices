using System;
using N8T.Domain;

namespace ProductCatalogService.Domain.Model
{
    public class Product : EntityBase, IAggregateRoot
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public double Price { get; set; }

        public string ImageUrl { get; set; } = default!;

        public Guid InventoryId { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}
