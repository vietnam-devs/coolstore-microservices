using System;

namespace N8T.Infrastructure.App.Dtos
{
    public class FlatCartDto
    {
        public Guid CartId { get; set; }
        public Guid UserId { get; set; }
        public double CartItemTotal { get; set; }
        public double CartItemPromoSavings { get; set; }
        public double ShippingTotal { get; set; }
        public double ShippingPromoSavings { get; set; }
        public double CartTotal { get; set; }
        public bool IsCheckOut { get; set; }

        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public double ProductPrice { get; set; }
        public string ProductDescription { get; set; } = default!;
        public string ProductImagePath { get; set; } = default!;

        public Guid InventoryId { get; set; }
        public string InventoryLocation { get; set; } = default!;
        public string InventoryWebsite { get; set; } = default!;
        public string InventoryDescription { get; set; } = default!;
    }
}
