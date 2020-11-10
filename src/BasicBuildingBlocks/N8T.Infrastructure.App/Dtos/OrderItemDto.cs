using System;
using OpenTelemetry.Trace;

namespace N8T.Infrastructure.App.Dtos
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public string InventoryFullInfo { get; set; } = default!;
    }
}
