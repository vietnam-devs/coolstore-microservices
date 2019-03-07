using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tanka.graphql;
using tanka.graphql.resolvers;
using VND.CoolStore.Services.Cart.v1.Grpc;
using VND.CoolStore.Services.Catalog.v1.Grpc;
using static tanka.graphql.resolvers.Resolve;
using VND.CoolStore.Services.GraphQL.v1.Types;
using VND.CoolStore.Services.Inventory.v1.Grpc;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class CoolStoreResolvers : ResolverMap
    {
        public CoolStoreResolvers(ICoolStoreResolverService resolverService)
        {
            this["Sample"] = new FieldResolverMap
            {
                {"name", PropertyOf<Sample>(m => m.Name)}
            };

            this["Product"] = new FieldResolverMap
            {
                {"id", PropertyOf<CatalogProductDto>(m => m.Id)},
                {"name", PropertyOf<CatalogProductDto>(m => m.Name)},
                {"price", PropertyOf<CatalogProductDto>(m => m.Price)},
                {"imageUrl", PropertyOf<CatalogProductDto>(m => m.ImageUrl)},
                {"desc", PropertyOf<CatalogProductDto>(m => m.Desc)}
            };

            this["Cart"] = new FieldResolverMap
            {
                {"id", PropertyOf<CartDto>(m => m.Id)},
                {"cartItemTotal", PropertyOf<CartDto>(m => m.CartItemTotal)},
                {"cartItemPromoSavings", PropertyOf<CartDto>(m => m.CartItemPromoSavings)},
                {"shippingTotal", PropertyOf<CartDto>(m => m.ShippingTotal)},
                {"shippingPromoSavings", PropertyOf<CartDto>(m => m.ShippingPromoSavings)},
                {"cartTotal", PropertyOf<CartDto>(m => m.CartTotal)},
                {"isCheckOut", PropertyOf<CartDto>(m => m.IsCheckOut)},
                {"items", PropertyOf<CartDto>(m => m.Items)}
            };

            this["CartItem"] = new FieldResolverMap
            {
                {"productId", PropertyOf<CartItemDto>(m => m.ProductId)},
                {"productName", PropertyOf<CartItemDto>(m => m.ProductName)},
                {"quantity", PropertyOf<CartItemDto>(m => m.Quantity)},
                {"price", PropertyOf<CartItemDto>(m => m.Price)},
                {"promoSavings", PropertyOf<CartItemDto>(m => m.PromoSavings)}
            };

            this["Inventory"] = new FieldResolverMap
            {
                {"id", PropertyOf<InventoryDto>(m => m.Id)},
                {"location", PropertyOf<InventoryDto>(m => m.Location)},
                {"quantity", PropertyOf<InventoryDto>(m => m.Quantity)},
                {"link", PropertyOf<InventoryDto>(m => m.Link)}
            };

            this["Query"] = new FieldResolverMap
            {
                {"samples", resolverService.GetSamplesAsync},
                {"products", resolverService.GetProductsAsync},
                {"product", resolverService.GetProductAsync},
                {"availabilities", resolverService.GetAvailabilitiesAsync},
                {"availability", resolverService.GetAvailabilityAsync},
                {"carts", resolverService.GetCartAsync},
            };
        }
    }

    public interface ICoolStoreResolverService
    {
        Task<IResolveResult> GetSamplesAsync(ResolverContext context);
        Task<IResolveResult> GetProductsAsync(ResolverContext context);
        Task<IResolveResult> GetProductAsync(ResolverContext context);
        Task<IResolveResult> GetCartAsync(ResolverContext context);
        Task<IResolveResult> GetAvailabilitiesAsync(ResolverContext context);
        Task<IResolveResult> GetAvailabilityAsync(ResolverContext context);
    }

    public class CoolStoreResolverService : ICoolStoreResolverService
    {
        public async Task<IResolveResult> GetSamplesAsync(ResolverContext context)
        {
            var page = context.Arguments["page"] ?? -1;
            return await new ValueTask<IResolveResult>(As(new List<Sample>
                {new Sample {Name = $"sample at page={page}"}}));
        }

        public async Task<IResolveResult> GetProductsAsync(ResolverContext context)
        {
            var currentPage = context.Arguments["currentPage"] ?? 1;
            var highPrice = context.Arguments["highPrice"] ?? 100;
            return await new ValueTask<IResolveResult>(
                As(new List<CatalogProductDto>
                    {new CatalogProductDto {Name = $"sample at currentPage={currentPage} and highPrice={highPrice}"}}));
        }

        public async Task<IResolveResult> GetProductAsync(ResolverContext context)
        {
            var id = context.Arguments["productId"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(As(new CatalogProductDto
                {Name = $"{id}"}));
        }

        public async Task<IResolveResult> GetCartAsync(ResolverContext context)
        {
            var id = context.Arguments["cartId"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(As(new CartDto {Id = $"{id}"}));
        }

        public async Task<IResolveResult> GetAvailabilitiesAsync(ResolverContext context)
        {
            return await new ValueTask<IResolveResult>(
                As(new List<InventoryDto>
                    {new InventoryDto {Id = $"{Guid.NewGuid()}"}}));
        }

        public async Task<IResolveResult> GetAvailabilityAsync(ResolverContext context)
        {
            var id = context.Arguments["inventoryId"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(As(new InventoryDto {Id = $"{id}"}));
        }
    }
}
