using System;
using N8T.Domain;

namespace ShoppingCartService.Domain.Event
{
    public class ShoppingCartCheckedOut : DomainEventBase
    {
        public Guid CartId { get; set; }
    }
}
