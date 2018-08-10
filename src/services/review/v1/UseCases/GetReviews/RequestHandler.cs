using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VND.CoolStore.Services.Review.Extensions;
using VND.Fw.Domain;
using VND.Fw.Infrastructure.AspNetCore.CleanArch;
using VND.Fw.Infrastructure.EfCore.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.GetReviews
{
  public class RequestHandler : RequestHandlerBase<GetReviewsRequest, GetReviewsResponse>
  {
    public RequestHandler(IUnitOfWorkAsync uow, IQueryRepositoryFactory qrf) : base(uow, qrf)
    {
    }

    public override async Task<GetReviewsResponse> Handle(GetReviewsRequest request,
      CancellationToken cancellationToken)
    {
      var reviewQueryRepo = QueryRepositoryFactory.QueryEfRepository<Domain.Review>();

      var reviews = await reviewQueryRepo
        .ListAsync(
          queryable => queryable
            .Include(x => x.ReviewAuthor)
            .Include(x => x.ReviewProduct))
        .ToObservable()
        .Select(x => x.Where(y => y.ReviewProduct.Id == request.ProductId));

      if (reviews == null)
      {
        throw new Exception($"Could not find a review with product#{request.ProductId}.");
      }

      return new GetReviewsResponse
      {
        Reviews = reviews
          .Select(x => x.ToDto())
          .ToList()
      };
    }
  }
}
