using GraphQL.Types;
using VND.CoolStore.Services.GraphQL.v1.Types;
using static VND.CoolStore.Services.Catalog.v1.Grpc.CatalogService;
using static VND.CoolStore.Services.Cart.v1.Grpc.CartService;
using static VND.CoolStore.Services.Inventory.v1.Grpc.InventoryService;
using static VND.CoolStore.Services.Rating.v1.Grpc.RatingService;
using static VND.CoolStore.Services.Review.v1.Grpc.ReviewService;

namespace VND.CoolStore.Services.GraphQL.v1
{
    public class CoolStoreQuery : ObjectGraphType<object>
    {
        private readonly CatalogServiceClient _catalogServiceClient;
        private readonly InventoryServiceClient _inventoryServiceClient;
        private readonly CartServiceClient _cartServiceClient;
        private readonly RatingServiceClient _ratingServiceClient;
        private readonly ReviewServiceClient _reviewServiceClient;

        public CoolStoreQuery(
            InventoryServiceClient inventoryServiceClient,
            CatalogServiceClient catalogServiceClient,
            CartServiceClient cartServiceClient,
            RatingServiceClient ratingServiceClient,
            ReviewServiceClient reviewServiceClient)
        {
            _catalogServiceClient = catalogServiceClient;
            _inventoryServiceClient = inventoryServiceClient;
            _cartServiceClient = cartServiceClient;
            _ratingServiceClient = ratingServiceClient;
            _reviewServiceClient = reviewServiceClient;

            Name = "Query";

            Field<SampleType>(
                "sample",
                resolve: context =>
                    new Sample {Name = "sample"});
        }
    }
}
