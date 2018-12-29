using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using NetCoreKit.Domain;
using VND.CoolStore.Services.Review.v1.Grpc;

namespace VND.CoolStore.Services.Review.v1.Services
{
    public class ReviewServiceImpl : ReviewService.ReviewServiceBase
    {
        private readonly IQueryRepositoryFactory _repositoryFactory;
        private readonly IUnitOfWorkAsync _uow;
        private readonly ILogger<ReviewServiceImpl> _logger;

        public ReviewServiceImpl(
            IQueryRepositoryFactory repositoryFactory,
            IUnitOfWorkAsync uow,
            ILoggerFactory loggerFactory)
        {
            _repositoryFactory = repositoryFactory;
            _uow = uow;
            _logger = loggerFactory.CreateLogger<ReviewServiceImpl>();
        }

        public override Task<GetReviewsResponse> GetReviews(GetReviewsRequest request, ServerCallContext context)
        {
            return base.GetReviews(request, context);
        }

        public override Task<CreateReviewResponse> CreateReview(CreateReviewRequest request, ServerCallContext context)
        {
            return base.CreateReview(request, context);
        }

        public override Task<EditReviewResponse> EditReview(EditReviewRequest request, ServerCallContext context)
        {
            return base.EditReview(request, context);
        }

        public override Task<DeleteReviewResponse> DeleteReview(DeleteReviewRequest request, ServerCallContext context)
        {
            return base.DeleteReview(request, context);
        }
    }
}
