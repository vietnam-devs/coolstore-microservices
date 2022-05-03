using ShoppingCart.Core.Dtos;

namespace ShoppingCart.UseCases.CreateShoppingCartWithProduct;

public class CreateShoppingCartWithProductCommand : IRequest<CartDto>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class CreateShoppingCartWithProductValidator : AbstractValidator<CreateShoppingCartWithProductCommand>
{
    public CreateShoppingCartWithProductValidator()
    {
    }
}
