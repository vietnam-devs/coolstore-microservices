using System;
using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Review.Extensions;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.AspNetCore.CleanArch;
using VND.Fw.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.UpdateReview
{
  public class RequestHandler : TxRequestHandlerBase<UpdateReviewRequest, UpdateReviewResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<UpdateReviewResponse> TxHandle(UpdateReviewRequest request,
      CancellationToken cancellationToken)
    {
      var reviewQueryRepo = QueryRepositoryFactory.QueryEfRepository<Domain.Review>();
      var reviewRepo = UnitOfWork.Repository<Domain.Review>();

      var review = await reviewQueryRepo.FindOneAsync(x => x.Id == request.ReviewId);
      if (review == null)
      {
        throw new Exception($"Could not find a review #{request.ReviewId}.");
      }

      review.Content = request.Content;
      var result = await reviewRepo.UpdateAsync(review);

      return new UpdateReviewResponse { Result = result.ToDto()};
    }
  }
}
