using ShoppingCart.Core.Dtos;
using ShoppingCart.Core.Gateways;
using ShoppingCart.Infrastructure.Extensions;

namespace ShoppingCart.UseCases.UpdateAmountOfProductInShoppingCart;

public class UpdateAmountOfProductInShoppingCartHandler : IRequestHandler<UpdateAmountOfProductInShoppingCartCommand, CartDto>
{
    private readonly DaprClient _daprClient;
    private readonly IProductCatalogGateway _productCatalogGateway;
    private readonly IPromoGateway _promoGateway;
    private readonly IShippingGateway _shippingGateway;
    private readonly ISecurityContextAccessor _securityContextAccessor;

    public UpdateAmountOfProductInShoppingCartHandler(DaprClient daprClient,
        IProductCatalogGateway productCatalogGateway,
        IPromoGateway promoGateway,
        IShippingGateway shippingGateway,
        ISecurityContextAccessor securityContextAccessor)
    {
        _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        _productCatalogGateway = productCatalogGateway ?? throw new ArgumentNullException(nameof(productCatalogGateway));
        _promoGateway = promoGateway ?? throw new ArgumentNullException(nameof(promoGateway));
        _shippingGateway = shippingGateway ?? throw new ArgumentNullException(nameof(shippingGateway));
        _securityContextAccessor = securityContextAccessor ?? throw new ArgumentNullException(nameof(securityContextAccessor));
    }

    public async Task<CartDto> Handle(UpdateAmountOfProductInShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _securityContextAccessor.UserId;

        var cart = await _daprClient.GetStateEntryAsync<CartDto>("statestore", $"shopping-cart-{currentUserId}",
            cancellationToken: cancellationToken);

        if (cart.Value is null)
        {
            throw new CoreException($"Couldn't find cart for user_id={currentUserId}");
        }

        var cartItem = cart.Value.Items.FirstOrDefault(x => x.ProductId == request.ProductId);

        // if not exists then it should be a new item
        if (cartItem is null)
        {
            await cart.Value.InsertItemToCartAsync(request.Quantity, request.ProductId, _productCatalogGateway);
        }
        else
        {
            cartItem.Quantity += request.Quantity;
        }

        await cart.Value.CalculateCartAsync(_productCatalogGateway, _shippingGateway, _promoGateway);

        await cart.SaveAsync(cancellationToken: cancellationToken);

        return cart.Value;
    }
}
