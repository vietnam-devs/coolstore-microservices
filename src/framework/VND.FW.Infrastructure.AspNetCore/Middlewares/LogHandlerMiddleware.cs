using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace VND.Fw.Infrastructure.AspNetCore.Middlewares
{
  public class LogHandlerMiddleware
  {
    private readonly ILogger<LogHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public LogHandlerMiddleware(ILogger<LogHandlerMiddleware> logger, RequestDelegate next)
    {
      _logger = logger;
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      context.Items["CorrelationId"] = Guid.NewGuid().ToString();
      _logger.LogInformation($"About to start {context.Request.Method} {context.Request.GetDisplayUrl()} request");

      await _next(context);

      _logger.LogInformation($"Request completed with status code: {context.Response.StatusCode} ");
    }
  }
}
