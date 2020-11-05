using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using MediatR;
using N8T.Infrastructure.App.Dtos;
using N8T.Infrastructure.Auth;

namespace ShoppingCartService.Application.GetCartByUserId
{
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

            var cart = await _daprClient.GetStateAsync<CartDto>("statestore", $"shopping-cart-{currentUserId}",
                cancellationToken: cancellationToken);

            if (cart is null)
            {
                cart = new CartDto();
                await _daprClient.SaveStateAsync("statestore", $"shopping-cart-{currentUserId}", cart,
                    cancellationToken: cancellationToken);
            }

            return cart;
        }
    }
}
