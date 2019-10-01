using System;
using System.ComponentModel.DataAnnotations.Schema;
using CloudNativeKit.Domain;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.Inventory.Domain
{
    [Table("Inventories", Schema = "inventory")]
    public class Inventory : AggregateRootBase<Guid>
    {
        private Inventory() : base(NewId())
        {
        }

        private Inventory(Guid id) : base(id)
        {
        }

        public string Location { get; private set; }

        public string Description { get; private set; }

        public string Website { get; private set; }
    }
}
