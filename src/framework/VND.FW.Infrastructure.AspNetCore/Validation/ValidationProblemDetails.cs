using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace VND.FW.Infrastructure.AspNetCore.Validation
{
  public class ValidationProblemDetails : ProblemDetails
  {
    public ICollection<ValidationError> ValidationErrors { get; set; }
  }
}
