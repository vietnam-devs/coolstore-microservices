using System;

namespace VND.Services.Inventory.UseCases.Entity
{
    public class Inventory
    {
        public Guid ItemId { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public string Link { get; set; }
    }
}