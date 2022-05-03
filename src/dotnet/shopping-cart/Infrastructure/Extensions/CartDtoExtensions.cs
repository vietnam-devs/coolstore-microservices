using ShoppingCart.Core.Dtos;
using ShoppingCart.Core.Exceptions;
using ShoppingCart.Core.Gateways;

namespace ShoppingCart.Infrastructure.Extensions;

public static class CartDtoExtensions
{
    public static async Task<CartDto> InsertItemToCartAsync(this CartDto cart,
        int quantity, Guid productId, IProductCatalogGateway productCatalogService)
    {
        var item = new CartItemDto {Quantity = quantity, ProductId = productId};

        var product = await productCatalogService.GetProductByIdAsync(productId);
        if (product is not null)
        {
            item.ProductName = product.Name;
            item.ProductPrice = product.Price;
            item.ProductImagePath = product.ImageUrl;
            item.ProductDescription = product.Description;
            item.InventoryId = product.InventoryId.ConvertTo<Guid>();
            item.InventoryLocation = product.InventoryLocation;
        }

        cart.Items.Add(item);

        return cart;
    }

    public static async Task<CartDto> CalculateCartAsync(this CartDto cart,
        IProductCatalogGateway productCatalogService,
        IShippingGateway shippingGateway,
        IPromoGateway promoGateway)
    {
        //DEMO: <tracing> temporary to slow-down the response
        // await Task.Delay(TimeSpan.FromSeconds(2));

        if (cart.Items.Count > 0)
        {
            cart.CartItemTotal = 0.0D;
            foreach (var cartItemTemp in cart.Items)
            {
                var product = await productCatalogService.GetProductByIdAsync(cartItemTemp.ProductId);
                if (product is null)
                {
                    throw new ProductNotFoundException(cartItemTemp.ProductId);
                }

                cart.CartItemPromoSavings += cartItemTemp.PromoSavings * cartItemTemp.Quantity;
                cart.CartItemTotal += product.Price * cartItemTemp.Quantity;
            }

            shippingGateway.CalculateShipping(cart);
        }

        promoGateway.ApplyShippingPromotions(cart);

        cart.CartTotal = cart.CartItemTotal + cart.ShippingTotal;

        return cart;
    }
}
