using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using VND.CoolStore.Shared.Catalog.GetProductById;
using VND.CoolStore.Shared.Catalog.GetProducts;
using VND.CoolStore.Shared.Rating.GetRatingByProductId;
using VND.CoolStore.Shared.Rating.GetRatings;
using VND.FW.Infrastructure.AspNetCore;
using VND.FW.Infrastructure.AspNetCore.Extensions;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Service.Impl
{
  public class RatingService : ProxyServiceBase, IRatingService
  {
    private readonly string _ratingServiceUri;

    public RatingService(
      RestClient rest,
      IConfiguration config,
      IHostingEnvironment env) : base(rest)
    {
      _ratingServiceUri = config.GetHostUri(env, "Rating");
    }

    public async Task<GetRatingByProductIdResponse> GetRatingByProductIdAsync(GetRatingByProductIdRequest request)
    {
      string getProductEndPoint = $"{_ratingServiceUri}/api/v1/ratings/{request.ProductId}";
      RestClient.SetOpenTracingInfo(request.Headers);
      GetRatingByProductIdResponse response = await RestClient.GetAsync<GetRatingByProductIdResponse>(getProductEndPoint);
      return response;
    }

    public async Task<IEnumerable<GetRatingsResponse>> GetRatingsAsync(GetRatingsRequest request)
    {
      string getProductsEndPoint = $"{_ratingServiceUri}/api/v1/ratings";
      RestClient.SetOpenTracingInfo(request.Headers);
      List<GetRatingsResponse> responses = await RestClient.GetAsync<List<GetRatingsResponse>>(getProductsEndPoint);
      return responses;
    }
  }
}
