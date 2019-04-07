using System.Linq;
using VND.CoolStore.Services.Cart.v1.Grpc;

namespace VND.CoolStore.Services.Cart.v1.Extensions
{
    public static class CartExtensions
    {
        public static CartDto ToDto(this Domain.Cart cart)
        {
            var cartDto = new CartDto
            {
                Id = cart.Id.ToString(),
                CartTotal = cart.CartTotal,
                CartItemTotal = cart.CartItemTotal,
                CartItemPromoSavings = cart.CartItemPromoSavings,
                ShippingPromoSavings = cart.ShippingPromoSavings,
                ShippingTotal = cart.ShippingTotal,
                IsCheckOut = cart.IsCheckout
            };

            cartDto.Items.AddRange(cart.CartItems.Select(cc => new CartItemDto
                {
                    ProductId = cc.Product.ProductId.ToString(),
                    ProductName = cc.Product.Name,
                    Price = cc.Price,
                    Quantity = cc.Quantity,
                    PromoSavings = cc.PromoSavings
                })
                .ToList());

            return cartDto;
        }
    }
}
