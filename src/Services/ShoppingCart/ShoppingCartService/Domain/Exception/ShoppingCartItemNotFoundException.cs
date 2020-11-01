using System;
using N8T.Domain;

namespace ShoppingCartService.Domain.Exception
{
    public class ShoppingCartItemNotFoundException : CoreException
    {
        public ShoppingCartItemNotFoundException(Guid id)
            : this($"The shopping cart item with id={id} is not found.")
        {

        }

        public ShoppingCartItemNotFoundException(string message) : base(message)
        {
        }
    }
}
