using System.Threading;
using System.Threading.Tasks;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using VND.CoolStore.Services.Review.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.AddReview
{
  public class RequestHandler : TxRequestHandlerBase<AddReviewRequest, AddReviewResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<AddReviewResponse> Handle(AddReviewRequest request,
      CancellationToken cancellationToken)
    {
      var reviewRepository = UnitOfWork.Repository<Domain.Review>();

      var review = Domain.Review.Load(request.Content)
        .AddAuthor(request.UserId)
        .AddProduct(request.ProductId);

      var result = await reviewRepository.AddAsync(review);
      await UnitOfWork.SaveChangesAsync(cancellationToken);

      return new AddReviewResponse {Result = result.ToDto()};
    }
  }
}
