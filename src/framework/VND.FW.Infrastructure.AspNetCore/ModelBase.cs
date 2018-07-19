using System;
using System.ComponentModel.DataAnnotations;

namespace VND.FW.Infrastructure.AspNetCore
{
  public abstract class ModelBase
  {
    [Required]
    public Guid Id { get; set; }
  }
}
