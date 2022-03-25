using ShoppingCart.Core.Dtos;

namespace ShoppingCart.Core.Events
{
    public class ShoppingCartCheckedOut : EventBase
    {
        public CartDto Cart { get; set; } = default!;

        public override void Flatten()
        {
            // implement it later
        }
    }
}
