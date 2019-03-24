using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.GrpcHost;
using NetCoreKit.Infrastructure.Mongo;
using NetCoreKit.Utils.Extensions;
using VND.CoolStore.Services.Review.v1.Extensions;
using VND.CoolStore.Services.Review.v1.Grpc;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace VND.CoolStore.Services.Review.v1.Services
{
    public class PingServiceImpl : PingService.PingServiceBase
    {
        private readonly ILogger<PingServiceImpl> _logger;

        public PingServiceImpl(IServiceProvider resolver)
        {
            _logger = resolver.GetService<ILoggerFactory>()?.CreateLogger<PingServiceImpl>();
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<PingResponse> Ping(Empty request, ServerCallContext context)
        {
            try
            {
                return await Task.FromResult(new PingResponse
                {
                    Message = $"Say hello from {Environment.MachineName} machine!!!"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
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

        [CheckPolicy("review_api_scope")]
        public override async Task<GetReviewsResponse> GetReviews(GetReviewsRequest request, ServerCallContext context)
        {
            try
            {
                var response = new GetReviewsResponse();
                var reviewQueryRepo = _queryFactory.MongoQueryRepository<Domain.Review>();
                var reviews =
                    await reviewQueryRepo.FindListByFieldAsync(r =>
                        r.ReviewProduct.Id == request.ProductId.ConvertTo<Guid>());

                if (reviews == null)
                    throw new CoreException($"Couldn't find the review with productId#{request.ProductId}.");

                response.Reviews.AddRange(
                    reviews
                        .Select(x => x.ToDto())
                        .ToList());

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<CreateReviewResponse> CreateReview(CreateReviewRequest request,
            ServerCallContext context)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<EditReviewResponse> EditReview(EditReviewRequest request, ServerCallContext context)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }

        [CheckPolicy("review_api_scope")]
        public override async Task<DeleteReviewResponse> DeleteReview(DeleteReviewRequest request,
            ServerCallContext context)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new RpcException(new Status(StatusCode.Internal, ex.Message));
            }
        }
    }
}
