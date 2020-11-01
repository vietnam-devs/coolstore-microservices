using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapr;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public async Task SubscribeProductsSync(ProductsState state, [FromServices] IMediator mediator)
        {
            _logger.LogInformation($"Received data for products-sync: {state.Products.Count} products.");

            var result = state;
        }
    }

    //TODO; refactor tomorrow
    public class ProductsState
    {
        public List<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
    
    public class ProductModel
    {
        public InventoryModel Inventory { get; set; }
        public CategoryModel Category { get; set; }
    }
    
    public class InventoryModel
    {
    
    }
    
    public class CategoryModel
    {
    
    }
}
