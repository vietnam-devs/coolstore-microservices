using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCoreKit.Infrastructure.AspNetCore.Authz;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using VND.CoolStore.Services.Cart.v1.UseCases.Checkout;
using VND.CoolStore.Services.Cart.v1.UseCases.DeleteItemInCart;
using VND.CoolStore.Services.Cart.v1.UseCases.GetCartById;
using VND.CoolStore.Services.Cart.v1.UseCases.InsertItemToNewCart;
using VND.CoolStore.Services.Cart.v1.UseCases.UpdateItemInCart;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace VND.CoolStore.Services.Cart.v1.UseCases
{
  [ApiVersion("1.0")]
  [Route("api/carts")]
  public class CartController : Controller
  {
    [HttpGet]
    [Route("{id}")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Get([FromServices] IMediator eventor, Guid id, CancellationToken cancellationToken)
    {
      return await eventor.SendStream<GetCartRequest, GetCartResponse>(
        new GetCartRequest { CartId = id },
        x => x.Result,
        cancellationToken);
    }

    [HttpPost]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Create([FromServices] IMediator eventor, InsertItemToNewCartRequest request, CancellationToken cancellationToken) =>
      await eventor.SendStream<InsertItemToNewCartRequest, InsertItemToNewCartResponse>(
        request,
        x => x.Result,
        cancellationToken);

    [HttpPut]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> Put([FromServices] IMediator eventor, UpdateItemInCartRequest request, CancellationToken cancellationToken) =>
      await eventor.SendStream<UpdateItemInCartRequest, UpdateItemInCartResponse>(
        request,
        x => x.Result,
        cancellationToken);

    [HttpDelete]
    [Route("{cartId:guid}/items/{productId:guid}")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> RemoveItemInCart([FromServices] IMediator eventor, Guid cartId, Guid productId, CancellationToken cancellationToken)
    {
      return await eventor.SendStream<DeleteItemRequest, DeleteItemResponse>(
        new DeleteItemRequest { CartId = cartId, ProductId = productId },
        x => x.ProductId,
        cancellationToken);
    }

    [HttpPut]
    [Route("{cartId:guid}/checkout")]
    [Auth(Policy = "access_cart_api")]
    public async Task<IActionResult> CheckoutCart([FromServices] IMediator eventor, Guid cartId, CancellationToken cancellationToken)
    {
      return await eventor.SendStream<CheckoutRequest, CheckoutResponse>(
        new CheckoutRequest { CartId = cartId },
        x => x.IsSucceed,
        cancellationToken);
    }
  }
}
