using System;
using System.Linq;
using System.Threading.Tasks;
using Coolstore;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.OpenApiV1.v1.Grpc;
using static Coolstore.CatalogService;

namespace VND.CoolStore.Services.OpenApiV1.v2
{
    [ApiVersion("2.0")]
    [Route("catalog/api/products")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogServiceClient _catalogServiceClient;

        public CatalogController(CatalogServiceClient catalogServiceClient)
        {
            _catalogServiceClient = catalogServiceClient;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpGet("admin-ping")]
        public IActionResult AdminPing()
        {
            return Ok();
        }

        [HttpGet("{currentPage:int}/{highPrice:int}")]
        public async ValueTask<IActionResult> GetProductWithPageAndPrice(int currentPage, int highPrice)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    var request = new GetProductsRequest
                    {
                        CurrentPage = currentPage,
                        HighPrice = highPrice
                    };
                    var response = await _catalogServiceClient.GetProductsAsync(request, headers);
                    var extraResponse = response.Products
                    .Select(x =>
                        new
                        {
                            x.Id,
                            x.Name,
                            x.Desc,
                            x.Price,
                            x.ImageUrl,
                            IsHot = new Random().Next() % 2 == 0
                        });

                    return Ok(extraResponse);
                });
        }

        [HttpGet("{productId:guid}")]
        public async ValueTask<IActionResult> GetProductById(Guid productId)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    var request = new GetProductByIdRequest
                    {
                        ProductId = productId.ToString()
                    };
                    var response = await _catalogServiceClient.GetProductByIdAsync(request, headers);
                    return Ok(response.Product);
                });
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateProduct(CreateProductRequest request)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "catalog-service",
                async headers =>
                {
                    var response = await _catalogServiceClient.CreateProductAsync(request, headers);
                    return Ok(response);
                });
        }
    }
}
