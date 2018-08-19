using System;
using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Review.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.UpdateReview
{
  public class RequestHandler : TxRequestHandlerBase<UpdateReviewRequest, UpdateReviewResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<UpdateReviewResponse> Handle(UpdateReviewRequest request,
      CancellationToken cancellationToken)
    {
      var reviewQueryRepo = QueryRepositoryFactory.QueryEfRepository<Domain.Review>();
      var reviewRepo = UnitOfWork.Repository<Domain.Review>();

      var review = await reviewQueryRepo.FindOneAsync(x => x.Id == request.ReviewId);
      if (review == null) throw new Exception($"Could not find a review #{request.ReviewId}.");

      review.Content = request.Content;
      var result = await reviewRepo.UpdateAsync(review);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new UpdateReviewResponse {Result = result.ToDto()};
    }
  }
}
