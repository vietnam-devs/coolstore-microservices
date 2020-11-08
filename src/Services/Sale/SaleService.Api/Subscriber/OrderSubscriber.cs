using System;
using System.Threading.Tasks;
using Dapr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N8T.Infrastructure.App.Events.ShoppingCart;
using SaleService.Application.CreateOrder;

namespace SaleService.Api.Subscriber
{
    [ApiController]
    [Route("")]
    public class OrderSubscriber : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrderSubscriber> _logger;

        public OrderSubscriber(IMediator mediator, ILogger<OrderSubscriber> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Topic("pubsub", "processing-order")]
        [HttpPost("processing-order")]
        public async Task SubscribeOrderProcessing(ShoppingCartCheckedOut @event)
        {
            _logger.LogInformation("{OrderSubscriber}: SubscribeOrderProcessing method.", nameof(OrderSubscriber));

            var query = new CreateOrderQuery {Cart = @event.Cart};

            await _mediator.Send(query);
        }
    }
}
