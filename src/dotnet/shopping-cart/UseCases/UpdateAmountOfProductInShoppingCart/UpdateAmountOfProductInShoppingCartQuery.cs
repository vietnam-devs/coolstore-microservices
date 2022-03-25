using ShoppingCart.Core.Dtos;

namespace ShoppingCart.UseCases.UpdateAmountOfProductInShoppingCart;

public class UpdateAmountOfProductInShoppingCartCommand : IRequest<CartDto>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}

public class UpdateAmountOfProductInShoppingCartValidator : AbstractValidator<UpdateAmountOfProductInShoppingCartCommand>
{
    public UpdateAmountOfProductInShoppingCartValidator()
    {
    }
}
