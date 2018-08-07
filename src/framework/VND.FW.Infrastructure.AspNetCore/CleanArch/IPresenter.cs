using System;
using Microsoft.AspNetCore.Mvc;

namespace VND.FW.Infrastructure.AspNetCore.CleanArch
{
  public interface IPresenter<TInput, TOutput>
  {
    TOutput Populate(Func<TInput> mapReturn = null);
  }

  public abstract class WebPresenterBase<TInput> : IPresenter<TInput, IActionResult>
  {
    public virtual IActionResult Populate(Func<TInput> mapReturn = null)
    {
      if (mapReturn == null)
      {
        return new NoContentResult();
      }

      return new OkObjectResult(mapReturn());
    }
  }

  public static class WebPresenterExtensions
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
