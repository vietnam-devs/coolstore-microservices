using System;
using System.Threading;
using System.Threading.Tasks;
using Dapr.Client;
using MediatR;
using N8T.Infrastructure.App.Dtos;

namespace ShoppingCartService.Application.GetCartByUserId
{
    public class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdQuery, CartDto>
    {
        private readonly DaprClient _daprClient;

        public GetCartByUserIdHandler(DaprClient daprClient)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
        }

        public async Task<CartDto> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
        {
            var cart = await _daprClient.GetStateAsync<CartDto>("statestore", $"shopping-cart-{request.UserId}",
                cancellationToken: cancellationToken) ?? new CartDto();

            return cart;
        }
    }
}
