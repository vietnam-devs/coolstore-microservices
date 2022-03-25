using ShoppingCart.Core.Dtos;

namespace ShoppingCart.UseCases.Checkout;

public class CheckOutCommand : IRequest<CartDto>
{
}

public class CheckOutValidator : AbstractValidator<CheckOutCommand>
{
}
