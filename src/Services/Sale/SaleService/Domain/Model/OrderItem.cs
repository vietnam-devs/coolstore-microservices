using System;
using N8T.Domain;

namespace SaleService.Domain.Model
{
    public class OrderItem : EntityBase
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; } = default!;
        public decimal Discount { get; set; } = default!;
        public Order Order { get; set; }
        public Guid OrderId { get; set; }
    }
}
