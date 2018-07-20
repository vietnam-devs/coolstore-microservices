using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using VND.CoolStore.Shared.Cart.InsertItemToNewCart;
using VND.CoolStore.Shared.Cart.UpdateItemInCart;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.EfCore.Extensions;
using VND.FW.Infrastructure.EfCore.Repository;

namespace VND.CoolStore.Services.Cart.UseCases.v1
{
		[ApiVersion("1.0")]
		[Route("api/v{api-version:apiVersion}/carts")]
		public class CartController : CrudControllerBase<Domain.Cart>
		{
				public CartController(
						IEfQueryRepository<Domain.Cart> queryRepository,
						IEfRepositoryAsync<Domain.Cart> mutateRepository)
						: base(queryRepository, mutateRepository)
				{
				}

				[HttpPost]
				[Route("new-cart")]
				public async Task<Domain.Cart> InsertItemToCart(InsertItemToNewCartRequest request)
				{
						Domain.Cart cart = new Domain.Cart
						{
								CartItemPromoSavings = 0
						};

						cart.InsertItemToCart(new Domain.CartItem()
						{
								ProductId = new Domain.ProductId(request.ItemId),
								PromoSavings = 0,
								Quantity = request.Quantity
						});

						return await MutateRepository.AddAsync(cart);
				}

				[HttpPut]
				[Route("update-cart")]
				public async Task<Domain.Cart> UpdateItemInCart(UpdateItemInCartRequest request)
				{
						var cart = await QueryRepository.GetByIdAsync(request.CartId);
						if(cart == null)
						{
								throw new Exception($"Could not find the cart[{request.CartId}]");
						}

						var item = cart.CartItems.FirstOrDefault(x => x.ProductId.Id == request.ItemId);
						if (item == null)
						{
								throw new Exception($"Could not find the item[{request.ItemId}]");
						}

						item.Quantity = request.Quantity;

						return await MutateRepository.UpdateAsync(cart);
				}

				[HttpDelete]
				[Route("{cartId:guid}/item/{itemId:guid}")]
				public async Task<bool> RemoveItemInCart(Guid cartId, Guid itemId)
				{
						Domain.Cart cart = await QueryRepository.GetByIdAsync(cartId);
						if (cart == null)
						{
								throw new Exception($"Could not find the cart[{cartId}]");
						}

						cart = cart.RemoveCartItem(itemId);
						bool isSucceed = await MutateRepository.UpdateAsync(cart) != null;

						return isSucceed;
				}
		}
}
