using System;
using N8T.Domain;

namespace ShoppingCartService.Domain.Exception
{
    public class ShoppingCartItemWithProductNotFoundException : CoreException
    {
        public ShoppingCartItemWithProductNotFoundException(Guid productId)
            : this($"The shopping cart item with product id={productId} is not found.")
        {

        }

        public ShoppingCartItemWithProductNotFoundException(string message) : base(message)
        {
        }
    }
}
