using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace VND.CoolStore.Services.ApiGateway.Infrastructure.Middlewares
{
	public class LoggingMiddleware
	{
		private readonly ILogger<LoggingMiddleware> logger;
		private readonly RequestDelegate next;

		public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
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
