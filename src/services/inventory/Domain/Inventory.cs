using System;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Inventory.Domain
{
    public class Inventory : EntityBase
    {
				internal Inventory() : base()
				{
				}

				public Inventory(Guid id) : base(id)
				{
				}

				public string Location { get; set; }
        public int Quantity { get; set; }
        public string Link { get; set; }
    }
}