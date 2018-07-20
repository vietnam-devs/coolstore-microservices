using System;
using System.ComponentModel.DataAnnotations;

namespace VND.FW.Infrastructure.AspNetCore
{
  public abstract class ModelBase
  {
  }

  public abstract class IdModelBase : ModelBase
  {
    [Required]
    public Guid Id { get; set; }
  }
}
