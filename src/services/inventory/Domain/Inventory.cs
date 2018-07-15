using VND.Fw.Domain;

namespace VND.CoolStore.Services.Inventory.Domain
{
    public class Inventory : EntityBase
    {
        public string Location { get; set; }
        public int Quantity { get; set; }
        public string Link { get; set; }
    }
}