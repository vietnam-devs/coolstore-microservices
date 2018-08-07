using System;
using Microsoft.AspNetCore.Mvc;

namespace VND.FW.Infrastructure.AspNetCore.CleanArch
{
  public static class PresenterExtensions
  {
    public static IActionResult PresentFor<TInput>(this TInput input, Func<TInput, dynamic> mapTo = null)
      where TInput : class
    {
      if (input == null)
      {
        return new NoContentResult();
      }

      if (mapTo == null)
      {
        return new OkObjectResult(input);
      }

      return new OkObjectResult(mapTo(input));
    }
  }
}
