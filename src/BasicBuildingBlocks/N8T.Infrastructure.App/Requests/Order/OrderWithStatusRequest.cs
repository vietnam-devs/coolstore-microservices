using System;

namespace N8T.Infrastructure.App.Requests.Order
{
    public class OrderWithStatusRequest
    {
        public Guid OrderId { get; set; }
        public int OrderStatus { get; set; }
    }
}
