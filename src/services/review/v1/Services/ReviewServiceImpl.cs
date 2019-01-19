using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.Mongo;
using NetCoreKit.Utils.Extensions;
using Newtonsoft.Json;
using VND.CoolStore.Services.Review.v1.Extensions;
using VND.CoolStore.Services.Review.v1.Grpc;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace VND.CoolStore.Services.Review.v1.Services
{
    public class PingServiceImpl : PingService.PingServiceBase
    {
        public override Task<PingResponse> Ping(Empty request, ServerCallContext context)
        {
            Console.WriteLine(JsonConvert.SerializeObject(context.AuthContext));
            Console.WriteLine(JsonConvert.SerializeObject(context.RequestHeaders));
            
            return Task.FromResult(new PingResponse
            {
                Message = $"Say hello from {Environment.MachineName} machine!!!"
            });
        }
    }

    public class ReviewServiceImpl : ReviewService.ReviewServiceBase
    {
        private readonly ILogger<ReviewServiceImpl> _logger;
        private readonly IQueryRepositoryFactory _queryFactory;
        private readonly IUnitOfWorkAsync _commandFactory;

        public ReviewServiceImpl(IServiceProvider resolver)
        {
            _logger = resolver.GetService<ILoggerFactory>()?.CreateLogger<ReviewServiceImpl>();

            _queryFactory = resolver.GetService<IQueryRepositoryFactory>();
            _commandFactory = resolver.GetService<IUnitOfWorkAsync>();
        }

        public override async Task<GetReviewsResponse> GetReviews(GetReviewsRequest request, ServerCallContext context)
        {
            var reviewQueryRepo = _queryFactory.MongoQueryRepository<Domain.Review>();
            var reviews =
                await reviewQueryRepo.FindListByFieldAsync(r =>
                    r.ReviewProduct.Id == request.ProductId.ConvertTo<Guid>());

            if (reviews == null)
                throw new CoreException($"Couldn't find the review with productId#{request.ProductId}.");

            var response = new GetReviewsResponse();
            response.Reviews.AddRange(
                reviews
                    .Select(x => x.ToDto())
                    .ToList());

            return response;
        }

        public override async Task<CreateReviewResponse> CreateReview(CreateReviewRequest request,
            ServerCallContext context)
        {
            var reviewRepository = _commandFactory.RepositoryAsync<Domain.Review>();

            var review = Domain.Review
                .Load(request.Content)
                .AddAuthor(request.UserId.ConvertTo<Guid>())
                .AddProduct(request.ProductId.ConvertTo<Guid>());

            var result = await reviewRepository.AddAsync(review);

            return new CreateReviewResponse
            {
                Result = result.ToDto()
            };
        }

        public override async Task<EditReviewResponse> EditReview(EditReviewRequest request, ServerCallContext context)
        {
            var reviewQueryRepo = _queryFactory.MongoQueryRepository<Domain.Review>();
            var reviewRepo = _commandFactory.RepositoryAsync<Domain.Review>();

            var review = await reviewQueryRepo.FindOneAsync(x => x.Id, request.ReviewId.ConvertTo<Guid>());
            if (review == null)
                throw new Exception($"Couldn't find the review #{request.ReviewId}.");

            review.Content = request.Content;
            var result = await reviewRepo.UpdateAsync(review);

            return new EditReviewResponse
            {
                Result = result.ToDto()
            };
        }

        public override async Task<DeleteReviewResponse> DeleteReview(DeleteReviewRequest request,
            ServerCallContext context)
        {
            var reviewQueryRepo = _queryFactory.MongoQueryRepository<Domain.Review>();
            var reviewRepo = _commandFactory.RepositoryAsync<Domain.Review>();

            var review = await reviewQueryRepo.FindOneAsync(x => x.Id, request.ReviewId.ConvertTo<Guid>());
            if (review == null)
                throw new Exception($"Couldn't find the review #{request.ReviewId}.");

            var result = await reviewRepo.DeleteAsync(review);

            return new DeleteReviewResponse
            {
                Id = result.Id.ToString()
            };
        }
    }
}
