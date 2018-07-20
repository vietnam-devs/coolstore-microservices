using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace VND.FW.Infrastructure.AspNetCore.Middlewares
{
  public class LogHandlerMiddleware
  {
    private readonly ILogger<LogHandlerMiddleware> logger;
    private readonly RequestDelegate next;

    public LogHandlerMiddleware(ILogger<LogHandlerMiddleware> logger, RequestDelegate next)
    {
      this.logger = logger;
      this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      context.Items["CorrelationId"] = Guid.NewGuid().ToString();
      logger.LogInformation($"About to start {context.Request.Method} {context.Request.GetDisplayUrl()} request");

      await next(context);

      logger.LogInformation($"Request completed with status code: {context.Response.StatusCode} ");
    }
  }
}
