using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using VND.Fw.Domain;

namespace VND.FW.Infrastructure.AspNetCore.Middlewares
{
  public class ErrorHandlerMiddleware
  {
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception exception)
      {
        await HandleErrorAsync(context, exception);
      }
    }

    private static Task HandleErrorAsync(HttpContext context, Exception exception)
    {
      string errorCode = "error";
      HttpStatusCode statusCode = HttpStatusCode.BadRequest;
      string message = "There was an error.";
      switch (exception)
      {
        case CoreException e:
          message = e.Message;
          break;
      }
      var response = new { code = errorCode, message = exception.Message };
      string payload = JsonConvert.SerializeObject(response);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = (int)statusCode;

      return context.Response.WriteAsync(payload);
    }
  }
}
