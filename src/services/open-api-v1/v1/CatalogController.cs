using System;
using System.Threading.Tasks;
using Coolstore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VND.CoolStore.Services.OpenApiV1.v1.Grpc;
using static Coolstore.CatalogService;
using static VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;

namespace VND.CoolStore.Services.OpenApiV1.v1
{
    [ApiVersion("1.0")]
    [Route("catalog/api/products")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ILogger<CatalogController> _logger;
        private readonly AppOptions _appOptions;
        private readonly CatalogServiceClient _catalogServiceClient;
        private readonly InventoryServiceClient _inventoryServiceClient;

        public CatalogController(
            ILoggerFactory loggerFactory,
            IOptions<AppOptions> options,
            CatalogServiceClient catalogServiceClient,
            InventoryServiceClient inventoryServiceClient)
        {
            _logger = loggerFactory.CreateLogger<CatalogController>();
            _appOptions = options.Value;
            _catalogServiceClient = catalogServiceClient;
            _inventoryServiceClient = inventoryServiceClient;
        }

        [HttpGet("ping")]
        public async ValueTask<IActionResult> Ping()
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    await _catalogServiceClient.PingAsync(
                        new Google.Protobuf.WellKnownTypes.Empty(),
                        headers/*,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut)*/);

                    return Ok();
                });
        }

        [HttpGet("admin-ping")]
        public async ValueTask<IActionResult> AdminPing()
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    await _catalogServiceClient.AdminPingAsync(
                        new Google.Protobuf.WellKnownTypes.Empty(),
                        headers/*,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut)*/);

                    return Ok();
                });
        }

        [HttpGet("expect-error")]
        public async ValueTask<IActionResult> ExpectError()
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    await _catalogServiceClient.ExpectErrorAsync(
                        new Google.Protobuf.WellKnownTypes.Empty(),
                        headers/*,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut)*/);

                    return Ok();
                });
        }

        [HttpGet("{currentPage:int}/{highPrice:int}")]
        public async ValueTask<IActionResult> GetProductWithPageAndPrice(int currentPage, int highPrice)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    _logger.LogInformation("headers:", headers);

                    var request = new GetProductsRequest
                    {
                        CurrentPage = currentPage,
                        HighPrice = highPrice
                    };

                    var response = await _catalogServiceClient.GetProductsAsync(
                        request,
                        headers/*,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut)*/);

                    return Ok(response.Products);
                });
        }

        [HttpGet("{productId:guid}")]
        public async ValueTask<IActionResult> GetProductById(Guid productId)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    _logger.LogInformation("headers:", headers);

                    var request = new GetProductByIdRequest
                    {
                        ProductId = productId.ToString()
                    };

                    var response = await _catalogServiceClient.GetProductByIdAsync(
                        request,
                        headers);

                    if (response?.Product == null)
                        throw new Exception($"Couldn't find product with id#{productId}.");

                    var inventory = await _inventoryServiceClient.GetInventoryAsync(
                        new Inventory.v1.Grpc.GetInventoryRequest
                        {
                            Id = response.Product.InventoryId
                        },
                        headers);

                    if (inventory == null)
                        throw new Exception($"Couldn't find inventory of product with id#{productId}.");

                    return Ok(new {
                        response.Product.Id,
                        response.Product.Name,
                        response.Product.Desc,
                        response.Product.Price,
                        response.Product.ImageUrl,
                        InventoryId = inventory.Result.Id,
                        InventoryLink = inventory.Result.Link,
                        InventoryLocation = inventory.Result.Location,
                    });
                });
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateProduct(CreateProductRequest request)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    _logger.LogInformation("headers:", headers);

                    var response = await _catalogServiceClient.CreateProductAsync(
                        request,
                        headers/*,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut)*/);

                    return Ok(response);
                });
        }
    }
}
