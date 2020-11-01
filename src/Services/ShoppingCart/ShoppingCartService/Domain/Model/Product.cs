using System;
using N8T.Domain;

namespace ShoppingCartService.Domain.Model
{
    public class Product : EntityBase
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public double Price { get; set; }

        public string Desc { get; set; } = default!;

        public string ImagePath { get; set; } = default!;

        public bool IsDeleted { get; set; }

        public Guid InventoryId { get; set; }
    }
}
