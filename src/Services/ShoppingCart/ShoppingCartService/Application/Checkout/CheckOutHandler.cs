using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using MediatR;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.App.Events.ShoppingCart;
using N8T.Infrastructure.Auth;

namespace ShoppingCartService.Application.Checkout
{
    public class CheckOutHandler : IRequestHandler<CheckOutQuery, CartDto>
    {
        private readonly DaprClient _daprClient;
        private readonly ISecurityContextAccessor _securityContextAccessor;

        public CheckOutHandler(DaprClient daprClient, ISecurityContextAccessor securityContextAccessor)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _securityContextAccessor = securityContextAccessor ?? throw new ArgumentNullException(nameof(securityContextAccessor));
        }

        public async Task<CartDto> Handle(CheckOutQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _securityContextAccessor.UserId;

            var cart = await _daprClient.GetStateEntryAsync<CartDto>("statestore", $"shopping-cart-{currentUserId}",
                cancellationToken: cancellationToken);

            cart.Value.UserId = currentUserId;
            var @event = new ShoppingCartCheckedOut {Cart = cart.Value};
            await _daprClient.PublishEventAsync("pubsub", "processing-order", @event, cancellationToken);

            cart.Value = new CartDto();
            await cart.SaveAsync(cancellationToken: cancellationToken);

            return cart.Value;
        }
    }
}
