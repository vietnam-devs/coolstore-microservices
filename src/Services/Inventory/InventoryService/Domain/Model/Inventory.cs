using System;
using N8T.Domain;

namespace InventoryService.Domain.Model
{
    public class Inventory : EntityBase, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Location { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Website { get; set; } = default!;
    }
}
