using System;
using System.Collections.Generic;

namespace N8T.Infrastructure.App.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerFullName { get; set; } = default!;
        public string CustomerEmail { get; set; } = default!;
        public string CustomerAddress { get; set; } = default!;
        public int OrderStatus { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string StaffFullName { get; set; } = default!;
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
