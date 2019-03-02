using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Model;

namespace WebUI.Services
{
  public class RatingService
  {
    public Task<IEnumerable<RatingModel>> GetRatings()
    {
      return Task.FromResult(new List<RatingModel>().AsEnumerable());
    }

    public Task<RatingModel> SetRating(Guid productId, Guid userId, int cost)
    {
      return Task.FromResult(new RatingModel());
    }
  }
}
