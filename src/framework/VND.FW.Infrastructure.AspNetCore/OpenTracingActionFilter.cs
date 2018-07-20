using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VND.FW.Infrastructure.AspNetCore
{
  public class OpenTracingActionFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      if (context.Controller is ProxyControllerBase)
      {
        var proxyController = context.Controller as ProxyControllerBase;
        // proxyController.InitRestClientWithOpenTracing();
      }
    }
  }
}
