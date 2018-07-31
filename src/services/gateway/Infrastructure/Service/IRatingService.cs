using System.Collections.Generic;
using System.Threading.Tasks;
using VND.CoolStore.Shared.Rating.GetRatingByProductId;
using VND.CoolStore.Shared.Rating.GetRatings;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Service
{
  public interface IRatingService
  {
    Task<GetRatingByProductIdResponse> GetRatingByProductIdAsync(GetRatingByProductIdRequest request);
    Task<IEnumerable<GetRatingsResponse>> GetRatingsAsync(GetRatingsRequest request);
  }
}
