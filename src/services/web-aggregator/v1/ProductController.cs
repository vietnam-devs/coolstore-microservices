using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Catalog.v1.Grpc;
using static VND.CoolStore.Services.Catalog.v1.Grpc.CatalogService;

namespace VND.CoolStore.Services.WebAggregator.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly CatalogServiceClient _serviceClient;

        public ProductController(CatalogServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        [HttpGet("{currentPage}/{highPrice}")]
        public async Task<IActionResult> Get(int currentPage, double highPrice)
        {
            var result = await _serviceClient.GetProductsAsync(new GetProductsRequest
            {
                CurrentPage = currentPage,
                HighPrice = highPrice
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _serviceClient.GetProductByIdAsync(new GetProductByIdRequest { ProductId = id.ToString() });
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var result = await _serviceClient.CreateProductAsync(request);
            return Ok(result);
        }
    }
}
