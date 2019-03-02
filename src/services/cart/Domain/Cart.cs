using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using static NetCoreKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.Services.Cart.Domain
{
    public sealed class Cart : AggregateRootBase
    {
        private Cart() : base(GenerateId())
        {
        }

        private Cart(Guid id) : base(id)
        {
        }

        [Required] public double CartItemTotal { get; private set; }

        [Required] public double CartItemPromoSavings { get; private set; }

        [Required] public double ShippingTotal { get; private set; }

        [Required] public double ShippingPromoSavings { get; private set; }

        [Required] public double CartTotal { get; private set; }

        [Required] public List<CartItem> CartItems { get; private set; } = new List<CartItem>();

        public bool IsCheckout { get; private set; }

        public static Cart Load()
        {
            return new Cart();
        }

        public static Cart Load(Guid id)
        {
            return new Cart(id);
        }

        public CartItem FindCartItem(Guid productId)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.ProductId == productId);
            return cartItem;
        }

        public Cart InsertItemToCart(Guid productId, int quantity, double promoSavings = 0.0D)
        {
            CartItems.Add(CartItem.Load(productId, quantity, 0.0D, promoSavings));
            return this;
        }

        public Cart RemoveCartItem(Guid itemId)
        {
            CartItems = CartItems.Where(y => y.Id != itemId).ToList();
            return this;
        }

        public Cart AccumulateCartItemQuantity(Guid cartItemId, int quantity)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Id == cartItemId);

            if (cartItem == null) throw new DomainException($"Couldn't find cart item #{cartItemId}");

            cartItem.AccumulateQuantity(quantity);
            return this;
        }

        public async Task<Cart> CalculateCartAsync(
            TaxType taxType, ICatalogGateway catalogGateway,
            IPromoGateway promoGateway, IShippingGateway shippingGateway)
        {
            if (CartItems != null && CartItems?.Count() > 0)
            {
                CartItemTotal = 0.0D;
                foreach (var cartItem in CartItems)
                {
                    var product = await catalogGateway.GetProductByIdAsync(cartItem.Product.ProductId);
                    if (product == null) throw new Exception("Could not find product.");

                    cartItem
                        .FillUpProductInfo(product.Name, product.Price, product.Desc)
                        .ChangePrice(product.Price);

                    CartItemPromoSavings = CartItemPromoSavings + cartItem.PromoSavings * cartItem.Quantity;
                    CartItemTotal = CartItemTotal + cartItem.Product.Price * cartItem.Quantity;
                }

                shippingGateway.CalculateShipping(this);
            }

            promoGateway.ApplyShippingPromotions(this);

            switch (taxType)
            {
                case TaxType.NoTax:
                    CartTotal = CartItemTotal + ShippingTotal;
                    break;
                case TaxType.TenPercentage:
                    var cartTotal = CartItemTotal + ShippingTotal;
                    CartTotal = cartTotal * 10 / 100 + cartTotal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(taxType), taxType, null);
            }

            return this;
        }

        public Cart Checkout()
        {
            IsCheckout = true;
            return this;
        }
    }
}
