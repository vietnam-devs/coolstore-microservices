using System;
using System.Threading;
using System.Threading.Tasks;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.AspNetCore.CleanArch;
using VND.Fw.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.RemoveReview
{
  public class RequestHandler : TxRequestHandlerBase<RemoveReviewRequest, RemoveReviewResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<RemoveReviewResponse> TxHandle(RemoveReviewRequest request,
      CancellationToken cancellationToken)
    {
      var reviewQueryRepo = QueryRepositoryFactory.QueryEfRepository<Domain.Review>();
      var reviewRepo = UnitOfWork.Repository<Domain.Review>();

      var review = await reviewQueryRepo.FindOneAsync(x => x.Id == request.ReviewId);
      if (review == null)
      {
        throw new Exception($"Could not find a review #{request.ReviewId}.");
      }

      var result = await reviewRepo.DeleteAsync(review);
      return new RemoveReviewResponse {ReviewId = result.Id};
    }
  }
}
