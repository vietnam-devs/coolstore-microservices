using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using MediatR;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.Auth;

namespace ShoppingCartService.Application.GetShoppingCartWithProducts
{
    public class GetShoppingCartWithProductsHandler : IRequestHandler<GetShoppingCartWithProductsQuery, IEnumerable<FlatCartDto>>
    {
        private readonly DaprClient _daprClient;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public GetShoppingCartWithProductsHandler(DaprClient daprClient, ISecurityContextAccessor securityContextAccessor)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _securityContextAccessor = securityContextAccessor ?? throw new ArgumentNullException(nameof(securityContextAccessor));
        }

        public async Task<IEnumerable<FlatCartDto>> Handle(GetShoppingCartWithProductsQuery request,
            CancellationToken cancellationToken)
        {
            var currentUserId = _securityContextAccessor.UserId;

            var cart = await _daprClient.GetStateAsync<CartDto>("statestore", $"shopping-cart-{currentUserId}",
                cancellationToken: cancellationToken);

            if (cart is null)
            {
                return new List<FlatCartDto>();
            }

            return cart.Items.Select(x => new FlatCartDto
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                CartItemTotal = cart.CartItemTotal,
                CartItemPromoSavings = cart.CartItemPromoSavings,
                ShippingTotal = cart.ShippingTotal,
                ShippingPromoSavings = cart.ShippingPromoSavings,
                CartTotal = cart.CartTotal,
                IsCheckOut = cart.IsCheckOut,
                Quantity = x.Quantity,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductPrice = x.ProductPrice,
                ProductDescription = x.ProductDescription,
                ProductImagePath = x.ProductImagePath,
                InventoryId = x.InventoryId,
                InventoryLocation = x.InventoryLocation,
                InventoryWebsite = x.InventoryWebsite,
                InventoryDescription = x.InventoryDescription
            });
        }
    }
}
