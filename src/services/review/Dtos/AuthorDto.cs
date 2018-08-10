using System;

namespace VND.CoolStore.Services.Review.Dtos
{
  public partial class ReviewDto
  {
    public class AuthorDto
    {
      public Guid Id { get; set; }
      public string UserName { get; set; } = "demouser";
    }
  }
}
