using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;

namespace N8T.Infrastructure.ErrorHandler
{
    /// <summary>
    /// ref to https://github.com/DamianEdwards/MinimalApiPlayground/blob/d9e1bcb46b/src/MinimalApiPlayground/Properties/ProblemDetailsDeveloperPageExceptionFilter.cs
    /// </summary>
    public class ProblemDetailsDeveloperPageExceptionFilter : IDeveloperPageExceptionFilter
    {
        private static readonly object ErrorContextItemsKey = new object();
        private static readonly MediaTypeHeaderValue JsonMediaType = new MediaTypeHeaderValue("application/json");

        private static readonly RequestDelegate RespondWithProblemDetails = RequestDelegateFactory.Create(
            (HttpContext context) =>
            {
                if (context.Items.TryGetValue(ErrorContextItemsKey, out var errorContextItem) &&
                    errorContextItem is ErrorContext errorContext)
                {
                    return new ErrorProblemDetailsResult(errorContext.Exception);
                }

                return null;
            }).RequestDelegate;

        public async Task HandleExceptionAsync(ErrorContext errorContext, Func<ErrorContext, Task> next)
        {
            var headers = errorContext.HttpContext.Request.GetTypedHeaders();
            var acceptHeader = headers.Accept;

            if (acceptHeader?.Any(h => h.IsSubsetOf(JsonMediaType)) == true)
            {
                errorContext.HttpContext.Items.Add(ErrorContextItemsKey, errorContext);
                await RespondWithProblemDetails(errorContext.HttpContext);
            }
            else
            {
                await next(errorContext);
            }
        }
    }

    internal class ErrorProblemDetailsResult : IResult
    {
        private readonly Exception _ex;

        public ErrorProblemDetailsResult(Exception ex)
        {
            _ex = ex;
        }

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            var problemDetails = new ProblemDetails
            {
                Title = $"An unhandled exception occurred while processing the request",
                Detail = $"{_ex.GetType().Name}: {_ex.Message}",
                Status = _ex switch
                {
                    BadHttpRequestException ex => ex.StatusCode,
                    _ => StatusCodes.Status500InternalServerError
                }
            };
            problemDetails.Extensions.Add("exception", _ex.GetType().FullName);
            problemDetails.Extensions.Add("stack", _ex.StackTrace);
            problemDetails.Extensions.Add("headers",
                httpContext.Request.Headers.ToDictionary(kvp => kvp.Key, kvp => (string)kvp.Value));
            problemDetails.Extensions.Add("routeValues", httpContext.GetRouteData().Values);
            problemDetails.Extensions.Add("query", httpContext.Request.Query);
            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                var routeEndpoint = endpoint as RouteEndpoint;
                var httpMethods = endpoint?.Metadata.GetMetadata<IHttpMethodMetadata>()?.HttpMethods;
                problemDetails.Extensions.Add("endpoint", new
                {
                    endpoint?.DisplayName,
                    routePattern = routeEndpoint?.RoutePattern.RawText,
                    routeOrder = routeEndpoint?.Order,
                    httpMethods = httpMethods != null ? string.Join(", ", httpMethods) : ""
                });
            }

            var result = Results.Json(problemDetails, statusCode: problemDetails.Status,
                contentType: "application/problem+json");

            await result.ExecuteAsync(httpContext);
        }
    }
}