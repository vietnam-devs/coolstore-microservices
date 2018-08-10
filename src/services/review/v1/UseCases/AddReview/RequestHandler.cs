using System.Threading;
using System.Threading.Tasks;
using VND.CoolStore.Services.Review.Extensions;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.AspNetCore.CleanArch;

namespace VND.CoolStore.Services.Review.v1.UseCases.AddReview
{
  public class RequestHandler : TxRequestHandlerBase<AddReviewRequest, AddReviewResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<AddReviewResponse> TxHandle(AddReviewRequest request,
      CancellationToken cancellationToken)
    {
      var reviewRepository = UnitOfWork.Repository<Domain.Review>();

      var review = Domain.Review.Load(request.Content)
          .AddAuthor(request.UserId)
          .AddProduct(request.ProductId);

      var result = await reviewRepository.AddAsync(review);
      return new AddReviewResponse { Result = result.ToDto()};
    }
  }
}
