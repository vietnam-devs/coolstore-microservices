using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using N8T.Domain;
using N8T.Infrastructure.App.Dtos;
using ShoppingCartService.Domain.Event;
using ShoppingCartService.Domain.Exception;
using ShoppingCartService.Domain.Gateway;
using ShoppingCartService.Domain.Service;

namespace ShoppingCartService.Domain.Model
{
    public class ShoppingCart : EntityBase, IAggregateRoot
    {
        public static ShoppingCart Load(Guid buyerId)
        {
            return new ShoppingCart {BuyerId = buyerId};
        }

        public Guid Id { get; set; }

        public Guid BuyerId { get; set; }

        public double CartItemTotal { get; set; }

        public double CartItemPromoSavings { get; set; }

        public double ShippingTotal { get; set; }

        public double ShippingPromoSavings { get; set; }

        public double CartTotal { get; set; }

        public bool IsCheckout { get; set; }

        public List<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCartItem FindCartItem(Guid productId)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.ProductId == productId);
            if (cartItem is null)
            {
                throw new ShoppingCartItemWithProductNotFoundException(productId);
            }

            return cartItem;
        }

        public ShoppingCart InsertItemToCart(Guid productId, int quantity, double promoSavings = 0.0D)
        {
            CartItems.Add(ShoppingCartItem.Load(Id, Guid.NewGuid(), productId, quantity, promoSavings));

            AddDomainEvent(new ShoppingCartWithProductCreated());

            return this;
        }

        public ShoppingCart RemoveCartItem(Guid itemId)
        {
            CartItems = CartItems.Where(y => y.Id != itemId).ToList();
            return this;
        }

        public ShoppingCart AccumulateCartItemQuantity(Guid cartItemId, int quantity)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Id == cartItemId);

            if (cartItem == null)
                throw new ShoppingCartItemNotFoundException(cartItemId);

            cartItem.AccumulateQuantity(quantity);
            return this;
        }

        public async Task<ShoppingCart> CalculateCartAsync(TaxType taxType, IProductCatalogService productCatalogService, IPromoGateway promoGateway, IShippingGateway shippingGateway)
        {
            if (CartItems.Count > 0)
            {
                CartItemTotal = 0.0D;
                foreach (var cartItem in CartItems)
                {
                    var product = await productCatalogService.GetProductByIdAsync(cartItem.ProductId);
                    if (product is null)
                    {
                        throw new ProductNotFoundException(cartItem.ProductId);
                    }

                    CartItemPromoSavings += cartItem.PromoSavings * cartItem.Quantity;
                    CartItemTotal += product.Price * cartItem.Quantity;
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

        public ShoppingCart Checkout()
        {
            IsCheckout = true;

            AddDomainEvent(new ShoppingCartCheckedOut { CartId = Id });

            return this;
        }

        public async Task<CartDto> ToDtoAsync(IProductCatalogService productCatalogService)
        {
            var cartDto = new CartDto
            {
                Id = Id,
                UserId = BuyerId,
                CartTotal = CartTotal,
                CartItemTotal = CartItemTotal,
                CartItemPromoSavings = CartItemPromoSavings,
                ShippingPromoSavings = ShippingPromoSavings,
                ShippingTotal = ShippingTotal,
                IsCheckOut = IsCheckout
            };

            var products = await productCatalogService.GetProductsAsync();

            cartDto.Items.AddRange(CartItems.Select(cc =>
            {
                var cartItem = new CartItemDto
                {
                    ProductId = cc.ProductId,
                    Quantity = cc.Quantity,
                    PromoSavings = cc.PromoSavings
                };

                var prod = products.FirstOrDefault(x => x.Id == cc.ProductId);
                if (prod is not null)
                {
                    cartItem.ProductName = prod.Name;
                    cartItem.ProductPrice = prod.Price;
                    cartItem.ProductImagePath = prod.ImageUrl;
                    cartItem.ProductDescription = prod.Description;
                    cartItem.Price = prod.Price;

                    cartItem.InventoryId = prod.Inventory.Id;
                    cartItem.InventoryLocation = prod.Inventory.Location;
                    cartItem.InventoryWebsite = prod.Inventory.Website;
                    cartItem.InventoryDescription = prod.Inventory.Description;
                }

                return cartItem;
            }).ToList());

            return cartDto;
        }
    }
}
