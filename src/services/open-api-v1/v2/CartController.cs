using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VND.CoolStore.Services.Cart.v1.Grpc;
using VND.CoolStore.Services.OpenApiV1.v1.Grpc;
using static VND.CoolStore.Services.Cart.v1.Grpc.CartService;

namespace VND.CoolStore.Services.OpenApiV1.v2
{
    [ApiVersion("2.0")]
    [Route("cart/api/carts")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppOptions _appOptions;
        private readonly CartServiceClient _cartServiceClient;

        public CartController(IOptions<AppOptions> options, CartServiceClient cartServiceClient)
        {
            _appOptions = options.Value;
            _cartServiceClient = cartServiceClient;
        }

        [HttpGet("ping")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("{cartId:guid}")]
        public async ValueTask<IActionResult> GetCart(Guid cartId)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "cart-service",
                async headers =>
                {
                    var request = new GetCartRequest
                    {
                        CartId = cartId.ToString()
                    };

                    var response = await _cartServiceClient.GetCartAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.Result);
                });
        }

        [HttpPost]
        public async ValueTask<IActionResult> InsertItemToCart(InsertItemToNewCartRequest request)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "cart-service",
                async headers =>
                {
                    var response = await _cartServiceClient.InsertItemToNewCartAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.Result);
                });
        }

        [HttpPut]
        public async ValueTask<IActionResult> UpdateItemInCart(UpdateItemInCartRequest request)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "cart-service",
                async headers =>
                {
                    var response = await _cartServiceClient.UpdateItemInCartAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.Result);
                });
        }

        [HttpDelete("{cartId:guid}/items/{productId:guid}")]
        public async ValueTask<IActionResult> DeleteCart(Guid cartId, Guid productId)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "cart-service",
                async headers =>
                {
                    var request = new DeleteItemRequest
                    {
                        CartId = cartId.ToString(),
                        ProductId = productId.ToString()
                    };

                    var response = await _cartServiceClient.DeleteItemAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.ProductId);
                });
        }

        [HttpPut("{cartId:guid}/checkout")]
        public async ValueTask<IActionResult> Checkout(Guid cartId)
        {
            return await HttpContext.EnrichGrpcWithHttpContext(
                "cart-service",
                async headers =>
                {
                    var request = new CheckoutRequest {
                        CartId = cartId.ToString()
                    };

                    var response = await _cartServiceClient.CheckoutAsync(
                        request,
                        headers,
                        DateTime.UtcNow.AddSeconds(_appOptions.GrpcTimeOut));

                    return Ok(response.IsSucceed);
                });
        }
    }
}
