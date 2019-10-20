using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNativeKit.Domain;
using VND.CoolStore.ShoppingCart.DataContracts.Dto.V1;
using VND.CoolStore.ShoppingCart.DataContracts.Event.V1;
using VND.CoolStore.ShoppingCart.Domain.ProductCatalog;
using static CloudNativeKit.Utils.Helpers.IdHelper;

namespace VND.CoolStore.ShoppingCart.Domain.Cart
{
    public sealed class Cart : AggregateRootBase<Guid>
    {
        private Cart() : base(NewId())
        {
        }

        private Cart(Guid id) : base(id)
        {
        }

        public static Cart Load(Guid userId)
        {
            return new Cart { UserId= userId };
        }

        public Guid UserId { get; private set; }

        public double CartItemTotal { get; private set; }

        public double CartItemPromoSavings { get; private set; }

        public double ShippingTotal { get; private set; }

        public double ShippingPromoSavings { get; private set; }

        public double CartTotal { get; private set; }

        public List<CartItem> CartItems { get; private set; } = new List<CartItem>();

        public bool IsCheckout { get; private set; }

        public CartItem FindCartItem(Guid productId)
        {
            var cartItem = CartItems.FirstOrDefault(x => x.Product.ProductId == productId);
            return cartItem;
        }

        public Cart InsertItemToCart(Guid productId, int quantity, double promoSavings = 0.0D)
        {
            CartItems.Add(CartItem.Load(EmptyId(), productId, quantity, promoSavings));
            AddEvent(new ShoppingCartWithProductCreated()); // todo: will remove soon
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

            if (cartItem == null)
                throw new DomainException($"Couldn't find cart item #{cartItemId}");

            cartItem.AccumulateQuantity(quantity);
            return this;
        }

        public async Task<Cart> CalculateCartAsync(TaxType taxType, IProductCatalogService productCatalogService, IPromoGateway promoGateway, IShippingGateway shippingGateway)
        {
            if (CartItems != null && CartItems?.Count() > 0)
            {
                CartItemTotal = 0.0D;
                foreach (var cartItem in CartItems)
                {
                    var product = await productCatalogService.GetProductByIdAsync(cartItem.Product.ProductId);
                    if (cartItem.Product == null)
                        throw new Exception("Could not find product.");

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

        public Cart Checkout()
        {
            IsCheckout = true;
            AddEvent(new ShoppingCartCheckedOut { CartId = Id.ToString() });
            return this;
        }

        public async Task<CartDto> ToDto(IProductCatalogService productCatalogService, IInventoryGateway inventoryGateway)
        {
            var cartDto = new CartDto
            {
                Id = Id.ToString(),
                UserId = UserId.ToString(),
                CartTotal = CartTotal,
                CartItemTotal = CartItemTotal,
                CartItemPromoSavings = CartItemPromoSavings,
                ShippingPromoSavings = ShippingPromoSavings,
                ShippingTotal = ShippingTotal,
                IsCheckOut = IsCheckout
            };

            var inventories = await inventoryGateway.GetAvailabilityInventories();

            cartDto.Items.AddRange(CartItems.Select(cc =>
            {
                var prod = productCatalogService.GetProductById(cc.Product.ProductId);
                var inventory = inventories.FirstOrDefault(x => x.Id == prod.InventoryId);

                var cartItem = new CartItemDto
                {
                    ProductId = cc.Product.ProductId.ToString(),
                    ProductName = prod.Name,
                    ProductPrice = prod.Price,
                    ProductImagePath = prod.ImagePath,
                    ProductDesc = prod.Desc,
                    Price = prod.Price,
                    Quantity = cc.Quantity,
                    PromoSavings = cc.PromoSavings
                };

                if(inventory != null)
                {
                    cartItem.InventoryId = inventory.Id;
                    cartItem.InventoryLocation = inventory.Location;
                    cartItem.InventoryWebsite = inventory.Website;
                    cartItem.InventoryDescription = inventory.Description;
                }

                return cartItem;
            }).ToList());

            return cartDto;
        }
    }
}
