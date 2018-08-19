using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCoreKit.Domain;
using NetCoreKit.Infrastructure.AspNetCore.CleanArch;
using NetCoreKit.Infrastructure.EfCore.Extensions;
using VND.CoolStore.Services.Review.Extensions;

namespace VND.CoolStore.Services.Review.v1.UseCases.GetReviews
{
  public class RequestHandler : RequestHandlerBase<GetReviewsRequest, GetReviewsResponse>
  {
    public RequestHandler(IQueryRepositoryFactory qrf) : base(qrf)
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
            .Include(x => x.ReviewProduct));

      var result = reviews.Where(x => x.ReviewProduct?.Id == request.ProductId);

      if (reviews == null) throw new Exception($"Couldn't find a review with product#{request.ProductId}.");

      return new GetReviewsResponse
      {
        Reviews = result
          .Select(x => x.ToDto())
          .ToList()
      };
    }
  }
}
