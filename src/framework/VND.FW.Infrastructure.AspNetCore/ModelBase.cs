using System;
using System.Collections.Generic;
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

  public abstract class RequestModelBase : ModelBase
  {
    public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
  }

  public abstract class RequestIdModelBase : IdModelBase
  {
    public IEnumerable<KeyValuePair<string, string>> Headers { get; set; }
  }
}
