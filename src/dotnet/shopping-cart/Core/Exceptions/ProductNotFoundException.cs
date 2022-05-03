namespace ShoppingCart.Core.Exceptions;

public class ProductNotFoundException : CoreException
{
    public ProductNotFoundException(Guid id)
        : this($"The product with id={id} is not found.")
    {

    }

    public ProductNotFoundException(string message) : base(message)
    {
    }
}
