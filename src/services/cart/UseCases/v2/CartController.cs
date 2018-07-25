using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VND.CoolStore.Services.Cart.Domain;
using VND.CoolStore.Services.Cart.Infrastructure.Service;
using VND.CoolStore.Shared.Cart.GetCartById;

namespace VND.CoolStore.Services.Cart.UseCases.v2
{
  [ApiVersion("2.0")]
  [Route("api/v{api-version:apiVersion}/carts")]
  public class CartController : FW.Infrastructure.AspNetCore.ControllerBase
  {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService, TenPercentTaxCalculator taxCaculator)
    {
      _cartService = cartService;
      _cartService.TaxCalculator = taxCaculator;
    }

    [HttpGet(Name = nameof(GetCartById))]
    [Route("{id}")]
    public async Task<GetCartByIdResponse> GetCartById(Guid id)
    {
      return await _cartService.GetCartByIdAsync(id);
    }
  }
}
