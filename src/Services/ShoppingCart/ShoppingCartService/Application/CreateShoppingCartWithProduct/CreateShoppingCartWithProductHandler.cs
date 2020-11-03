using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using MediatR;
using N8T.Infrastructure.App.Dtos;
using ShoppingCartService.Domain.Gateway;
using ShoppingCartService.Domain.Model;
using ShoppingCartService.Domain.Service;

namespace ShoppingCartService.Application.CreateShoppingCartWithProduct
{
    public class CreateShoppingCartWithProductHandler : IRequestHandler<CreateShoppingCartWithProductQuery, CartDto>
    {
        private readonly DaprClient _daprClient;
        private readonly IProductCatalogService _productCatalogService;
        private readonly IPromoGateway _promoGateway;
        private readonly IShippingGateway _shippingGateway;

        public CreateShoppingCartWithProductHandler(DaprClient daprClient,
            IProductCatalogService productCatalogService,
            IPromoGateway promoGateway,
            IShippingGateway shippingGateway)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _productCatalogService = productCatalogService ?? throw new ArgumentNullException(nameof(productCatalogService));
            _promoGateway = promoGateway ?? throw new ArgumentNullException(nameof(promoGateway));
            _shippingGateway = shippingGateway ?? throw new ArgumentNullException(nameof(shippingGateway));
        }

        public async Task<CartDto> Handle(CreateShoppingCartWithProductQuery request,
            CancellationToken cancellationToken)
        {
            var cart = await ShoppingCart.Load(request.UserId)
                .InsertItemToCart(request.ProductId, request.Quantity)
                .CalculateCartAsync(
                    TaxType.NoTax,
                    _productCatalogService,
                    _promoGateway,
                    _shippingGateway);

            await _daprClient.SaveStateAsync("statestore", $"shopping-cart-{request.UserId}", cart,
                cancellationToken: cancellationToken);

            return await cart.ToDtoAsync(_productCatalogService);
        }
    }
}
