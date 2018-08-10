using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using VND.FW.Infrastructure.AspNetCore.Extensions;
using VND.FW.Infrastructure.AspNetCore.Middlewares;

namespace VND.FW.Infrastructure.AspNetCore.Miniservice
{
  public class ProblemDetailsConfigureApplication : IConfigureApplication
  {
    public int Priority { get; } = 200;
    public void Configure(IApplicationBuilder app)
    {
      var env = app.ApplicationServices.GetRequiredService<IHostingEnvironment>();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseDatabaseErrorPage();
        // app.UseMiniProfiler();
      }
      else
      {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseExceptionHandler(errorApp =>
      {
#pragma warning disable CS1998
        errorApp.Run(async context =>
          {
            var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
            var exception = errorFeature.Error;

            // the IsTrusted() extension method doesn't exist and
            // you should implement your own as you may want to interpret it differently
            // i.e. based on the current principal

            var problemDetails = new ProblemDetails
            {
              Instance = $"urn:myorganization:error:{Guid.NewGuid()}"
            };

            if (exception is BadHttpRequestException badHttpRequestException)
            {
              problemDetails.Title = "Invalid request";
              problemDetails.Status = (int)typeof(BadHttpRequestException).GetProperty(
                  "StatusCode", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(badHttpRequestException);
              problemDetails.Detail = badHttpRequestException.Message;
            }
            else
            {
              problemDetails.Title = "An unexpected error occurred!";
              problemDetails.Status = 500;
              problemDetails.Detail = exception.Demystify().ToString();
            }

            // TODO: log the exception etc..

            context.Response.StatusCode = problemDetails.Status.Value;
            context.Response.WriteJson(problemDetails, "application/problem+json");
          }
#pragma warning restore CS1998
        );
      });

      app.UseMiddleware<ErrorHandlerMiddleware>();
    }
  }
}