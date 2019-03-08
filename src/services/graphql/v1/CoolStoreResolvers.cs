using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tanka.graphql;
using tanka.graphql.resolvers;
using VND.CoolStore.Services.Cart.v1.Grpc;
using VND.CoolStore.Services.Catalog.v1.Grpc;
using VND.CoolStore.Services.Inventory.v1.Grpc;
using VND.CoolStore.Services.Rating.v1.Grpc;
using VND.CoolStore.Services.Review.v1.Grpc;
using static tanka.graphql.resolvers.Resolve;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class CoolStoreResolvers : ResolverMap
    {
        public CoolStoreResolvers(ICoolStoreResolverService resolverService)
        {
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

            this["Rating"] = new FieldResolverMap
            {
                {"id", PropertyOf<RatingDto>(m => m.Id)},
                {"productId", PropertyOf<RatingDto>(m => m.ProductId)},
                {"userId", PropertyOf<RatingDto>(m => m.UserId)},
                {"cost", PropertyOf<RatingDto>(m => m.Cost)}
            };

            this["Review"] = new FieldResolverMap
            {
                {"id", PropertyOf<ReviewDto>(m => m.Id)},
                {"content", PropertyOf<ReviewDto>(m => m.Content)},
                {"authorId", PropertyOf<ReviewDto>(m => m.AuthorId)},
                {"authorName", PropertyOf<ReviewDto>(m => m.AuthorName)},
                {"productId", PropertyOf<ReviewDto>(m => m.ProductId)},
                {"productName", PropertyOf<ReviewDto>(m => m.ProductName)}
            };

            this["Query"] = new FieldResolverMap
            {
                {"products", resolverService.GetProductsAsync},
                {"product", resolverService.GetProductAsync},
                {"carts", resolverService.GetCartAsync},
                {"availabilities", resolverService.GetAvailabilitiesAsync},
                {"availability", resolverService.GetAvailabilityAsync},
                {"ratings", resolverService.GetRatingsAsync},
                {"rating", resolverService.GetRatingAsync},
                {"reviews", resolverService.GetReviewsAsync}
            };

            this["Mutation"] = new FieldResolverMap
            {
                {"createProduct",  resolverService.CreateProductAsync},
                {"insertItemToNewCart",  resolverService.InsertItemToNewCartAsync},
                {"updateItemInCart",  resolverService.UpdateItemInCartAsync},
                {"deleteItem",  resolverService.DeleteItemAsync},
                {"checkout",  resolverService.CheckoutAsync},
                {"createRating",  resolverService.CreateRatingAsync},
                {"updateRating",  resolverService.UpdateRatingAsync},
                {"createReview",  resolverService.CreateReviewAsync},
                {"editReview",  resolverService.EditReviewAsync},
                {"deleteReview",  resolverService.DeleteReviewAsync}
            };
        }
    }

    public interface ICoolStoreResolverService
    {
        ValueTask<IResolveResult> GetProductsAsync(ResolverContext context);
        ValueTask<IResolveResult> GetProductAsync(ResolverContext context);
        ValueTask<IResolveResult> CreateProductAsync(ResolverContext context);
        ValueTask<IResolveResult> InsertItemToNewCartAsync(ResolverContext context);
        ValueTask<IResolveResult> UpdateItemInCartAsync(ResolverContext context);
        ValueTask<IResolveResult> DeleteItemAsync(ResolverContext context);
        ValueTask<IResolveResult> CheckoutAsync(ResolverContext context);
        ValueTask<IResolveResult> GetCartAsync(ResolverContext context);
        ValueTask<IResolveResult> GetAvailabilitiesAsync(ResolverContext context);
        ValueTask<IResolveResult> GetAvailabilityAsync(ResolverContext context);
        ValueTask<IResolveResult> GetRatingsAsync(ResolverContext context);
        ValueTask<IResolveResult> GetRatingAsync(ResolverContext context);
        ValueTask<IResolveResult> CreateRatingAsync(ResolverContext context);
        ValueTask<IResolveResult> UpdateRatingAsync(ResolverContext context);
        ValueTask<IResolveResult> GetReviewsAsync(ResolverContext context);
        ValueTask<IResolveResult> CreateReviewAsync(ResolverContext context);
        ValueTask<IResolveResult> EditReviewAsync(ResolverContext context);
        ValueTask<IResolveResult> DeleteReviewAsync(ResolverContext context);
    }

    public class CoolStoreResolverService : ICoolStoreResolverService
    {
        public async ValueTask<IResolveResult> GetProductsAsync(ResolverContext context)
        {
            var currentPage = context.Arguments["currentPage"] ?? 1;
            var highPrice = context.Arguments["highPrice"] ?? 100;
            return await new ValueTask<IResolveResult>(
                As(new List<CatalogProductDto>
                    {new CatalogProductDto {Name = $"sample at currentPage={currentPage} and highPrice={highPrice}"}}));
        }

        public async ValueTask<IResolveResult> GetProductAsync(ResolverContext context)
        {
            var id = context.Arguments["_id"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(As(new CatalogProductDto
                {Name = $"{id}"}));
        }

        public async ValueTask<IResolveResult> CreateProductAsync(ResolverContext context)
        {
            var name = context.GetArgument<string>("name");
            var price = context.GetArgument<double>("price");
            var desc = context.GetArgument<string>("desc");
            var imageUrl = context.GetArgument<string>("imageUrl");

            var product = new CatalogProductDto
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Price = price,
                Desc = desc,
                ImageUrl = imageUrl
            };
            return await Task.FromResult(As(product));
        }

        public async ValueTask<IResolveResult> InsertItemToNewCartAsync(ResolverContext context)
        {
            var productId = context.GetArgument<string>("productId");
            var quantity = context.GetArgument<int>("quantity");

            return await Task.FromResult(As(new CartDto()));
        }

        public async ValueTask<IResolveResult> UpdateItemInCartAsync(ResolverContext context)
        {
            var cartId = context.GetArgument<string>("cartId");
            var productId = context.GetArgument<string>("productId");
            var quantity = context.GetArgument<int>("quantity");

            return await Task.FromResult(As(new CartDto()));
        }

        public async ValueTask<IResolveResult> DeleteItemAsync(ResolverContext context)
        {
            var cartId = context.GetArgument<string>("cartId");
            var productId = context.GetArgument<string>("productId");

            return await Task.FromResult(As(cartId));
        }

        public async ValueTask<IResolveResult> CheckoutAsync(ResolverContext context)
        {
            var cartId = context.GetArgument<string>("cartId");

            return await Task.FromResult(As(true));
        }

        public async ValueTask<IResolveResult> GetCartAsync(ResolverContext context)
        {
            var id = context.Arguments["_id"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(As(new CartDto {Id = $"{id}"}));
        }

        public async ValueTask<IResolveResult> GetAvailabilitiesAsync(ResolverContext context)
        {
            return await new ValueTask<IResolveResult>(
                As(new List<InventoryDto>
                    {new InventoryDto {Id = $"{Guid.NewGuid()}"}}));
        }

        public async ValueTask<IResolveResult> GetAvailabilityAsync(ResolverContext context)
        {
            var id = context.Arguments["_id"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(As(new InventoryDto {Id = $"{id}"}));
        }

        public async ValueTask<IResolveResult> GetRatingsAsync(ResolverContext context)
        {
            return await new ValueTask<IResolveResult>(
                As(new List<RatingDto>
                    {new RatingDto {Id = $"{Guid.NewGuid()}"}}));
        }

        public async ValueTask<IResolveResult> GetRatingAsync(ResolverContext context)
        {
            var productId = context.Arguments["productId"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(As(new InventoryDto { Id = $"{productId}" }));
        }

        public async ValueTask<IResolveResult> CreateRatingAsync(ResolverContext context)
        {
            var productId = context.GetArgument<string>("productId");
            var userId = context.GetArgument<string>("userId");
            var cost = context.GetArgument<double>("cost");

            return await Task.FromResult(As(new RatingDto()));
        }

        public async ValueTask<IResolveResult> UpdateRatingAsync(ResolverContext context)
        {
            var id = context.GetArgument<string>("id");
            var productId = context.GetArgument<string>("productId");
            var userId = context.GetArgument<string>("userId");
            var cost = context.GetArgument<double>("cost");

            return await Task.FromResult(As(new RatingDto()));
        }

        public async ValueTask<IResolveResult> GetReviewsAsync(ResolverContext context)
        {
            var productId = context.Arguments["productId"] ?? Guid.NewGuid();
            return await new ValueTask<IResolveResult>(
                As(new List<RatingDto>
                    {new RatingDto {Id = $"{productId}"}}));
        }

        public async ValueTask<IResolveResult> CreateReviewAsync(ResolverContext context)
        {
            var productId = context.GetArgument<string>("productId");
            var userId = context.GetArgument<string>("userId");
            var content = context.GetArgument<string>("content");

            return await Task.FromResult(As(new ReviewDto()));
        }

        public async ValueTask<IResolveResult> EditReviewAsync(ResolverContext context)
        {
            var id = context.GetArgument<string>("id");
            var content = context.GetArgument<string>("content");

            return await Task.FromResult(As(new ReviewDto()));
        }

        public async ValueTask<IResolveResult> DeleteReviewAsync(ResolverContext context)
        {
            var id = context.GetArgument<string>("id");

            return await Task.FromResult(As(id));
        }
    }
}
