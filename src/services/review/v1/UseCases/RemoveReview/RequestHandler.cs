using System;
using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.RemoveReview
{
  public class RequestHandler : TxRequestHandlerBase<RemoveReviewRequest, RemoveReviewResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<RemoveReviewResponse> Handle(RemoveReviewRequest request,
      CancellationToken cancellationToken)
    {
      var reviewQueryRepo = QueryFactory.QueryEfRepository<Domain.Review>();
      var reviewRepo = CommandFactory.Repository<Domain.Review>();

      var review = await reviewQueryRepo.FindOneAsync(x => x.Id == request.ReviewId);
      if (review == null) throw new Exception($"Couldn't find a review #{request.ReviewId}.");
      var result = await reviewRepo.DeleteAsync(review);

      return new RemoveReviewResponse {ReviewId = result.Id};
    }
  }
}
