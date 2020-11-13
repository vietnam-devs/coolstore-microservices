using System;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ShoppingCartService.Api.Subscriber
{
    [ApiController]
    [Route("")]
    public class ProductSubscriber : ControllerBase
    {
        private readonly DaprClient _daprClient;
        private readonly ILogger<ProductSubscriber> _logger;

        public ProductSubscriber(DaprClient daprClient, ILogger<ProductSubscriber> logger)
        {
            _daprClient = daprClient ?? throw new ArgumentNullException(nameof(daprClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
