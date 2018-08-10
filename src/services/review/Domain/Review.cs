using System;
using System.ComponentModel.DataAnnotations;
using VND.Fw.Domain;

namespace VND.CoolStore.Services.Review.Domain
{
  public class Review : EntityBase
  {
    internal Review(string content) : base(Guid.NewGuid())
    {
      Content = content;
    }

    public Review(Guid id, string content) : base(id)
    {
      Content = content;
    }

    public static Review Load(string content)
    {
      return new Review(content);
    }

    public static Review Load(Guid id, string content)
    {
      return new Review(id, content);
    }

    [Required]
    public string Content { get; set; }
    [Required]
    public ReviewAuthor ReviewAuthor { get; set; }
    [Required]
    public ReviewProduct ReviewProduct { get; set; }

    public Review AddAuthor(Guid userId)
    {
      ReviewAuthor = new ReviewAuthor(userId);
      return this;
    }

    public Review AddProduct(Guid productId)
    {
      ReviewProduct = new ReviewProduct(productId);
      return this;
    }
  }
}
