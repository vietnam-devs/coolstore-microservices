using N8T.Domain;
using N8T.Infrastructure.App.Dtos;

namespace N8T.Infrastructure.App.Events.ShoppingCart
{
    public class ShoppingCartCheckedOut : DomainEventBase
    {
        public CartDto Cart { get; set; } = default!;
    }
}
