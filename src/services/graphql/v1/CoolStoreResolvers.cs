using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using tanka.graphql;
using tanka.graphql.error;
using tanka.graphql.resolvers;
using VND.CoolStore.Services.Cart.v1.Grpc;
using VND.CoolStore.Services.Catalog.v1.Grpc;
using VND.CoolStore.Services.Inventory.v1.Grpc;
using VND.CoolStore.Services.Rating.v1.Grpc;
using static tanka.graphql.resolvers.Resolve;
using static VND.CoolStore.Services.Cart.v1.Grpc.CartService;
using static VND.CoolStore.Services.Catalog.v1.Grpc.CatalogService;
using static VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;
using static VND.CoolStore.Services.Rating.v1.Grpc.RatingService;

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

      this["Query"] = new FieldResolverMap
            {
                {"products", resolverService.GetProductsAsync},
                {"product", resolverService.GetProductAsync},
                {"carts", resolverService.GetCartAsync},
                {"availabilities", resolverService.GetAvailabilitiesAsync},
                {"availability", resolverService.GetAvailabilityAsync},
                {"ratings", resolverService.GetRatingsAsync},
                {"rating", resolverService.GetRatingAsync},
            };

      this["Mutation"] = new FieldResolverMap
            {
                {"createProduct", resolverService.CreateProductAsync},
                {"insertItemToNewCart", resolverService.InsertItemToNewCartAsync},
                {"updateItemInCart", resolverService.UpdateItemInCartAsync},
                {"deleteItem", resolverService.DeleteItemAsync},
                {"checkout", resolverService.CheckoutAsync},
                {"createRating", resolverService.CreateRatingAsync},
                {"updateRating", resolverService.UpdateRatingAsync},
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
  }

  public class CoolStoreResolverService : ICoolStoreResolverService
  {
    private readonly IHttpContextAccessor _httpContext;
    private readonly CatalogServiceClient _catalogServiceClient;
    private readonly CartServiceClient _cartServiceClient;
    private readonly InventoryServiceClient _inventoryServiceClient;
    private readonly RatingServiceClient _ratingServiceClient;
    public CoolStoreResolverService(
        IHttpContextAccessor httpContext,
        CatalogServiceClient catalogServiceClient,
        CartServiceClient cartServiceClient,
        InventoryServiceClient inventoryServiceClient,
        RatingServiceClient ratingServiceClient)
    {
      _httpContext = httpContext;
      _catalogServiceClient = catalogServiceClient;
      _cartServiceClient = cartServiceClient;
      _inventoryServiceClient = inventoryServiceClient;
      _ratingServiceClient = ratingServiceClient;
    }

    public async ValueTask<IResolveResult> GetProductsAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "catalog-service",
          async headers =>
          {
            var input = context.GetArgument<GetProductsRequest>("input");
            var results = await _catalogServiceClient.GetProductsAsync(input, headers);
            return As(results.Products);
          });
    }

    public async ValueTask<IResolveResult> GetProductAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "catalog-service",
          async headers =>
          {
            var input = context.GetArgument<GetProductByIdRequest>("input");
            var result = await _catalogServiceClient.GetProductByIdAsync(input, headers);
            return As(result.Product);
          });
    }

    public async ValueTask<IResolveResult> CreateProductAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "catalog-service",
          async headers =>
          {
            var input = context.GetArgument<CreateProductRequest>("input");
            var result = await _catalogServiceClient.CreateProductAsync(input, headers);
            return As(result.Product);
          });
    }

    public async ValueTask<IResolveResult> InsertItemToNewCartAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "cart-service",
          async headers =>
          {
            var input = context.GetArgument<InsertItemToNewCartRequest>("input");
            var result = await _cartServiceClient.InsertItemToNewCartAsync(input, headers);
            return As(result.Result);
          });
    }

    public async ValueTask<IResolveResult> UpdateItemInCartAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "cart-service",
          async headers =>
          {
            var input = context.GetArgument<UpdateItemInCartRequest>("input");
            var result = await _cartServiceClient.UpdateItemInCartAsync(input, headers);
            return As(result.Result);
          });
    }

    public async ValueTask<IResolveResult> DeleteItemAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "cart-service",
          async headers =>
          {
            var input = context.GetArgument<DeleteItemRequest>("input");
            var result = await _cartServiceClient.DeleteItemAsync(input, headers);
            return As(result.ProductId);
          });
    }

    public async ValueTask<IResolveResult> CheckoutAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "cart-service",
          async headers =>
          {
            var input = context.GetArgument<CheckoutRequest>("input");
            var result = await _cartServiceClient.CheckoutAsync(input, headers);
            return As(result.IsSucceed);
          });
    }

    public async ValueTask<IResolveResult> GetCartAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "cart-service",
          async headers =>
          {
            var input = context.GetArgument<GetCartRequest>("input");
            var result = await _cartServiceClient.GetCartAsync(input, headers);
            return As(result.Result);
          });
    }

    public async ValueTask<IResolveResult> GetAvailabilitiesAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "inventory-service",
          async headers =>
          {
            var result = await _inventoryServiceClient.GetInventoriesAsync(new Empty(), headers);
            return As(result.Inventories);
          });
    }

    public async ValueTask<IResolveResult> GetAvailabilityAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "inventory-service",
          async headers =>
          {
            var input = context.GetArgument<GetInventoryRequest>("input");
            var result = await _inventoryServiceClient.GetInventoryAsync(input, headers);
            return As(result.Result);
          });
    }

    public async ValueTask<IResolveResult> GetRatingsAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "rating-service",
          async headers =>
          {
            var result = await _ratingServiceClient.GetRatingsAsync(new Empty(), headers);
            return As(result.Ratings);
          });
    }

    public async ValueTask<IResolveResult> GetRatingAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "rating-service",
          async headers =>
          {
            var input = context.GetArgument<GetRatingByProductIdRequest>("input");
            var result = await _ratingServiceClient.GetRatingByProductIdAsync(input, headers);
            return As(result.Rating);
          });
    }

    public async ValueTask<IResolveResult> CreateRatingAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "rating-service",
          async headers =>
          {
            var input = context.GetArgument<CreateRatingRequest>("input");
            var result = await _ratingServiceClient.CreateRatingAsync(input, headers);
            return As(result.Rating);
          });
    }

    public async ValueTask<IResolveResult> UpdateRatingAsync(ResolverContext context)
    {
      return await GrpcClientCatch(
          "rating-service",
          async headers =>
          {
            var input = context.GetArgument<UpdateRatingRequest>("input");
            var result = await _ratingServiceClient.UpdateRatingAsync(input, headers);
            return As(result.Rating);
          });
    }

    private async ValueTask<IResolveResult> GrpcClientCatch(string scope,
        Func<Metadata, ValueTask<IResolveResult>> catchAction)
    {
      try
      {
        var metadata = new Metadata();
        if (_httpContext.HttpContext.Request != null
            && _httpContext.HttpContext.Request.Headers.Any(
                h => h.Key.ToLower().Contains("authorization")))
        {
          metadata.Add("authorization", _httpContext.HttpContext.Request?.Headers["authorization"]);
        }

        // add supporting for OpenTracing standard
        _httpContext.HttpContext.Request?.Headers.ToList().ForEach(h =>
        {
          if (h.Key == "x-request-id" ||
                      h.Key == "x-b3-traceid" ||
                      h.Key == "x-b3-spanid" ||
                      h.Key == "x-b3-parentspanid" ||
                      h.Key == "x-b3-sampled" ||
                      h.Key == "x-b3-flags" ||
                      h.Key == "x-ot-span-context")
          {
            metadata.Add(h.Key, h.Value);
          }
        });

        return await catchAction(metadata);
      }
      catch (RpcException ex)
      {
        throw new GraphQLError($"{scope}: {ex.Message}");
      }
    }
  }
}
