using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using N8T.Domain;
using ShoppingCartService.Domain.Dto;

namespace ShoppingCartService.Api.Subscriber
{
    [ApiController]
    [Route("")]
    public class ProductSubscriber : ControllerBase
    {
        private readonly ILogger<ProductSubscriber> _logger;

        public ProductSubscriber(ILogger<ProductSubscriber> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Topic("pubsub", "products-sync")]
        [HttpPost("products-sync")]
        public async Task SubscribeProductsSync(ProductListReplicated @event, [FromServices] IMediator mediator)
        {
            _logger.LogInformation($"Received data for products-sync: {@event.Products.Count} products.");

            var result = @event;
        }
    }

    public class ProductListReplicated : IntegrationEventBase
    {
        public List<ProductDto> Products { get; set; } = new List<ProductDto>();
    }
}
