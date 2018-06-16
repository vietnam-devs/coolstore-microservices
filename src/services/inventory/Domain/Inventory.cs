using System;

namespace VND.CoolStore.Services.Inventory.Domain
{
    public class Inventory
    {
        public Guid ItemId { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public string Link { get; set; }
    }
}