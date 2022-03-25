namespace ShoppingCart.Core.Exceptions;

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
