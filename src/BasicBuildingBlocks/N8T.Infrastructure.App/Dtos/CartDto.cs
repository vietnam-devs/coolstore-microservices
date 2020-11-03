using System;
using System.Collections.Generic;

namespace N8T.Infrastructure.App.Dtos
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double CartItemTotal { get; set; }
        public double CartItemPromoSavings { get; set; }
        public double ShippingTotal { get; set; }
        public double ShippingPromoSavings { get; set; }
        public double CartTotal { get; set; }
        public bool IsCheckOut { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
