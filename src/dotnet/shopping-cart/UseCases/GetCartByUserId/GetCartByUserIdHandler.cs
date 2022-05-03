using N8T.Infrastructure.Auth;
using ShoppingCart.Core.Dtos;

namespace ShoppingCart.UseCases.GetCartByUserId;

public class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdQuery, CartDto>
{
    private readonly DaprClient _daprClient;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public GetCartByUserIdHandler(DaprClient daprClient, ISecurityContextAccessor securityContextAccessor)
    {
        _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        _securityContextAccessor = securityContextAccessor ?? throw new ArgumentNullException(nameof(securityContextAccessor));
    }

    public async Task<CartDto> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = _securityContextAccessor.UserId;

        var cart = await _daprClient.GetStateEntryAsync<CartDto>("statestore", $"shopping-cart-{currentUserId}",
            cancellationToken: cancellationToken);

        if (cart.Value is not null) return cart.Value;

        cart.Value = new CartDto();
        await cart.SaveAsync(cancellationToken: cancellationToken);

        return cart.Value;
    }
}
