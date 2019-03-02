using System;

namespace WebUI.Model
{
  public class RatingModel
  {
    public Guid ProductId { get; set; }
    public Guid UserId { get; set; }
    public int Cost { get; set; }
  }
}
